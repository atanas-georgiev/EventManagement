import { AdminDetailsComponent } from './components/admindetails/admindetails.component';
import { RegisterService } from './services/register.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { DetailsComponent } from './components/details/details.component';
import { EventService } from './services/event.service';
import { UserService } from './services/user.service';

import { GridComponent } from 'gijgo-angular-wrappers';
// import * as types from 'gijgo';

@NgModule({
    declarations: [
        AppComponent,
        GridComponent,
        HomeComponent,
        DetailsComponent,
        AdminDetailsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        BrowserModule,
        RouterModule.forRoot([
            { path: "home", component: HomeComponent },
            { path: "details/:id", component: DetailsComponent },
            { path: "registrations/:id", component: AdminDetailsComponent },
            { path: "", component: HomeComponent  },
        ])
    ],
    providers: [
        EventService,
        UserService,
        RegisterService
    ]
})
export class AppModuleShared {
}
