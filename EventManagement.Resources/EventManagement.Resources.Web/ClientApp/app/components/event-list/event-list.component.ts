import { Component, ViewChild, OnInit } from '@angular/core';
import { GridComponent } from 'gijgo-angular-wrappers';
import * as GijgoTypes from 'gijgo';
import { IEvent } from '../../models/event.model';
import { EventService } from "../../services/event.service";
import { IError } from "../../models/error.model";
import { Router } from "@angular/router";

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

    private events: IEvent[] = [];

    @ViewChild("grid") grid: GridComponent<IEvent, Params>;

    configuration: GijgoTypes.GridSettings<IEvent>;

    error: IError | null;

    columns: Array<GijgoTypes.GridColumn> = [
        { field: 'id', width: 86 },
        { field: 'name', title: 'Name' },
        { field: 'lecturerName', title: 'Lecturer' },
        { field: 'start', title: 'Start', type: 'date', format: 'dd mmm yyyy HH:MM' },
        { field: 'end', title: 'End', type: 'date', format: 'dd mmm yyyy HH:MM' },
        { width: 100, field: 'price', title: 'Price'  },
        { width: 60, tmpl: '<span class="glyphicon glyphicon-search"></span>', align: 'center', events: { 'click': (e: any) => this.navigateDetails(e) } },
        { width: 60, tmpl: '<span class="glyphicon glyphicon-remove"></span>', align: 'center', events: { 'click': (e: any) => this.remove(e) } }
    ];

    constructor(private eventService : EventService, private router: Router) {
        this.configuration = {
            // uiLibrary: 'bootstrap4',
            columns: this.columns,
            primaryKey: 'Id',
            pager: { limit: 5 }
        };
    }

    ngOnInit() {
        this.eventService.getAll().subscribe(x => {
            this.events = x;
            for (let ev of this.events) {
                this.grid.instance.addRow(ev);
            }
        });
    }

    remove(e: any) {
        if (!confirm("Are you sure?")) {
            return;
        }

        if (e.data) {
            this.eventService.remove(e.data.record.id).subscribe(
                data => console.log(data),
                err => {
                    this.error = <IError>({
                        message: err.text().replace(/"/g, ''),
                        entity: e.data.record as IEvent
                    });
            });
        }
    }

    navigateDetails(e: any) {
        this.router.navigate(['/events/' + e.data.record.id]);
    }


}

interface Params {
    page?: number
}