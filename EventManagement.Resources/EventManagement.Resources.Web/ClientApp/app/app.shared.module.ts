import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { ResourceService } from './services/resource.service';
import { ResourceListComponent } from './components/resource-list/resource-list.component';
import { ResourceAddComponent } from './components/resource-add/resource-add.component';
import { EventListComponent } from './components/event-list/event-list.component';
import { EventService } from './services/event.service';
import { EventDetailComponent } from './components/event-detail/event-detail.component';
import { EventAddComponent } from './components/event-add/event-add.component';

import { GridComponent, CheckBoxComponent, DatePickerComponent, DialogComponent, DropDownComponent, EditorComponent, TimePickerComponent, TreeComponent } from 'gijgo-angular-wrappers';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        FetchDataComponent,
        HomeComponent,
        ResourceListComponent,
        ResourceAddComponent,
        EventListComponent,
        EventDetailComponent,
        EventAddComponent,
        GridComponent,
        CheckBoxComponent,
        DatePickerComponent,
        DialogComponent,
        DropDownComponent,
        EditorComponent,
        TimePickerComponent,
        TreeComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: ResourceListComponent },
            { path: 'list', component: ResourceListComponent },
            { path: 'add', component: ResourceAddComponent },
            { path: 'events-list', component: EventListComponent },
            { path: 'events/:id', component: EventDetailComponent },
            { path: 'events/add/res/:id', component: EventAddComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        ResourceService,
        EventService
    ]
})
export class AppModuleShared {
}
