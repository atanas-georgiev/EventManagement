import { UserService } from './../../services/user.service';
import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as $ from 'jquery';
import { GridComponent } from 'gijgo-angular-wrappers';
import * as GijgoTypes from 'gijgo';
import { IEvent } from '../../models/event.model';
import { EventService } from '../../services/event.service';
import { Router } from '@angular/router';
import { IParams } from '../../models/params.model';

@Component({
    templateUrl: './home.component.html'
})
export class HomeComponent {
    @ViewChild("grid") grid: GridComponent<IEvent, IParams>;

    configuration: GijgoTypes.GridSettings<IEvent>;

    columns: Array<GijgoTypes.GridColumn> = [
        { field: 'eventId', hidden: true },
        { field: 'eventName', title: "Event" },
        { field: 'location', title: "Location" },
        { field: 'start', title: "Date", type: 'date', format: 'yyyy-mm-dd' }, // , format: 'HH:MM:ss mm/dd/yyyy'
        { field: 'resourcePlacesCount', title: "Free Places" },
        { width: 64, tmpl: '<span class="glyphicon glyphicon-info-sign gj-cursor-pointer" title="Information"> </span>', align: 'center', events: { 'click': (e: any) => this.navigate(e) } },
        { field: 'isAdmin', title: "", width: 64, hidden: true, tmpl: '<span class="glyphicon glyphicon-user gj-cursor-pointer" title="Registrations"> </span>', align: 'center', events: { 'click': (e: any) => this.navigateAdmin(e) } }
    ];

    constructor(private resourceService: EventService, private userService: UserService, private router: Router) { }

    ngOnInit() {

        this.configuration = {
            columns: this.columns,
            primaryKey: 'eventId',
            pager: { limit: 10 },
            autoLoad: true
        };

        this.resourceService.getAll().subscribe(x => {
            for (let res of x) {
                this.grid.instance.addRow(res);
            }
        });
    }

    ngAfterViewInit() {
        this.userService.isAdmin().subscribe(isAdmin => {
            if (isAdmin) {
                this.grid.instance.showColumn("isAdmin");
            }
        });
    }

    navigate(e: any) {
        this.router.navigate(["details/" + e.data.record["eventId"]]);
    }

    navigateAdmin(e: any) {
        this.router.navigate(["registrations/" + e.data.record["eventId"]]);
    }
}