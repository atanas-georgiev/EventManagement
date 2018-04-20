import { GridComponent } from 'gijgo-angular-wrappers';
import { IRegistrationUser } from './../../models/registrationuser.mode';
import { RegisterService } from './../../services/register.service';
import { Component, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { EventService } from "../../services/event.service";
import { UserService } from "../../services/user.service";
import { IEvent } from "../../models/event.model";
import { DatePipe } from "@angular/common";
import { CurrencyPipe } from "@angular/common";
import { Location } from "@angular/common";
import * as GijgoTypes from 'gijgo';
import { IParams } from '../../models/params.model';

@Component({
    templateUrl: "./admindetails.component.html"
})
export class AdminDetailsComponent {
    private event: IEvent;
    private registrations: Array<string> = new Array<string>();

    constructor(private route: ActivatedRoute,
        private eventService: EventService,
        private userService: UserService,
        private location: Location,
        private registerService: RegisterService) { }

    ngOnInit() {
        this.route.params.forEach((params: Params) => {
            const id: number = params["id"] as number;

            this.eventService.get(id).subscribe(x => {
                this.event = x;
            });

            this.registerService.get(id).subscribe(x => {
                for (let res of x) {
                    this.registrations.push(res.userName);
                }
            });
        });
    }
    
    back() {
        this.location.back();
    }
}
