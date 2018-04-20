import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from "@angular/forms";
import { IEvent } from '../../models/event.model';
import { EventService } from "../../services/event.service";
import { ResourceService } from "../../services/resource.service";
import { ActivatedRoute } from "@angular/router";
import { IResource } from "../../models/resource.model";
import { IError } from "../../models/error.model";


@Component({
    selector: 'app-event-add',
    templateUrl: './event-add.component.html',
    styleUrls: ['./event-add.component.css']
})
export class EventAddComponent implements OnInit {
    @Input() event: IEvent;
    resource: IResource;
    error: IError | null;

    eventForm = new FormGroup({
        additionalInfo: new FormControl(),
        lecturerName: new FormControl(),
        name: new FormControl(),
        price: new FormControl(),
        end: new FormControl(),
        startDate: new FormControl(),
        startTime: new FormControl(),
        endDate: new FormControl(),
        endTime: new FormControl(),
    });

    constructor(private eventService: EventService, private resourceService: ResourceService, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            let currId: number = params['id'];
            this.resourceService.get(currId).subscribe(r => {
                this.resource = r as IResource;
            });
        });
    }

    onSubmit() {
        if (!this.eventForm.valid) {
            for (let controlName in this.eventForm.controls) {
                if (!this.eventForm.controls[controlName].valid) {
                    this.eventForm.controls[controlName].markAsDirty();
                }
            }
            return;
        }

        if (confirm("Are you sure?")) {
            this.validateDates();

            if (this.eventForm.valid) {
                this.dismissError();
            } else {
                this.error = <IError>{
                    message: 'The fields are not filled correctly',
                    entity: null
                }
                return;
            }

            let startDateTime: Date = new Date(this.eventForm.value["startDate"] + ' ' + this.eventForm.value["startTime"]);
            let endDateTime: Date = new Date(this.eventForm.value["endDate"] + ' ' + this.eventForm.value["endTime"]);

            let event = <IEvent>({
                additionalInfo: this.eventForm.value["additionalInfo"],
                lecturerName: this.eventForm.value["lecturerName"],
                name: this.eventForm.value["name"],
                price: this.eventForm.value["price"] || 0,
                start: startDateTime,
                end: endDateTime,
                resourceId: this.resource.id
            });
            this.eventService.create(event).subscribe(data =>
            {
                this.event = data as IEvent;
                this.eventForm.reset();
            });
        }
    }

    dismissError() {
        this.error = null;
    }

    validateDates() {
        let startDateTime: Date = new Date(this.eventForm.value["startDate"] + ' ' + this.eventForm.value["startTime"]);
        let endDateTime: Date = new Date(this.eventForm.value["endDate"] + ' ' + this.eventForm.value["endTime"]);
        let message: string = '';

        if (startDateTime > endDateTime || startDateTime === endDateTime) {
            if (startDateTime === endDateTime) {
                message = 'The start date cannot equal the end date';
            } else {
                message = 'The start date cannot be after the end date';
            }
            
            this.error = <IError>{
                message: message,
                entity: null
            }

            this.eventForm.controls["startDate"].setErrors({ 'incorrect': true });
            this.eventForm.controls["startTime"].setErrors({ 'incorrect': true });
            this.eventForm.controls["endDate"].setErrors({ 'incorrect': true });
            this.eventForm.controls["endTime"].setErrors({ 'incorrect': true });
            return;
        } else {
            this.dismissError();
            this.eventForm.controls["startDate"].setErrors(null);
            this.eventForm.controls["startTime"].setErrors(null);
            this.eventForm.controls["endDate"].setErrors(null);
            this.eventForm.controls["endTime"].setErrors(null);
        }
    }
}
