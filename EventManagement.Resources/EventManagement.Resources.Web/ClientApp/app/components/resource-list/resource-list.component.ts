import { Component, ViewChild, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as $ from 'jquery';
import { GridComponent } from 'gijgo-angular-wrappers';
import * as GijgoTypes from 'gijgo';
import { IResource } from '../../models/resource.model';
import { ResourceService } from '../../services/resource.service';
import { IError } from "../../models/error.model";
import { Router } from "@angular/router";

@Component({
    selector: 'app-resource-list',
    templateUrl: './resource-list.component.html',
    styleUrls: ['./resource-list.component.css']
})
export class ResourceListComponent implements OnInit {

    private resources: IResource[] = [];

    @ViewChild("grid") grid: GridComponent<IResource, Params>;

    configuration: GijgoTypes.GridSettings<IResource>;

    error: IError | null;

    columns: Array<GijgoTypes.GridColumn> = [
        { field: 'id', width: 86 },
        { field: 'name', title: 'Name' },
        { field: 'location', title: 'Location' },
        { width: 64, tmpl: '<span class="glyphicon glyphicon-remove"></span>', align: 'center', events: { 'click': (e: any) => this.remove(e) }, cssClass: 'pseudo-link' },
        { width: 120, tmpl: '<span class="glyphicon glyphicon-plus"></span> Add event', align: 'center', events: { 'click': (e: any) => this.addEvent(e) }, cssClass: 'pseudo-link' }
    ];

    constructor(private resourceService: ResourceService, private router: Router) {
        this.configuration = {
            // uiLibrary: 'bootstrap4',
            columns: this.columns,
            primaryKey: 'Id',
            pager: { limit: 5 }
        };
    }

    ngOnInit() {
        this.resourceService.getAll().subscribe(x => {
            this.resources = x;
            
            for (let res of this.resources) {
                this.grid.instance.addRow(res);
            }
        });
    }

    remove(e: any) {
        if (e.data) {            
            this.resourceService.remove(e.data.record.id).subscribe(
                data => console.log(data),
                err => {
                    this.grid.instance.reload();
                    this.error = <IError>({
                        message: err.text().replace(/"/g, ''),
                        entity: e.data.record as IResource
                    });
                });
        }
    }

    dismissError() {
        this.error = null;
    }

    addEvent(e: any) {
        this.router.navigate(['/events/add/res/' + e.data.record.id]);
    }
}

interface Params {
    page?: number
}
