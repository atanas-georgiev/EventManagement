import { RegisterService } from './../../services/register.service';
import { Component, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { EventService } from "../../services/event.service";
import { UserService } from "../../services/user.service";
import { IEvent } from "../../models/event.model";
import { DatePipe } from "@angular/common";
import { CurrencyPipe } from "@angular/common";
import { Location } from "@angular/common";

@Component({
    templateUrl: "./details.component.html"
})
export class DetailsComponent {
    private event: IEvent;
    private user: string = "";

    constructor(private route: ActivatedRoute,
        private eventService: EventService,
        private userService: UserService,
        private location: Location,
        private registerService: RegisterService) { }

    ngOnInit() {
        this.userService.get().subscribe(x => {
            this.user = x;
        });

        this.route.params.forEach((params: Params) => {
            const id: number = params["id"] as number;

            this.eventService.get(id).subscribe(x => {
                this.event = x;
                console.log(this.event);
            });
        });
    }

    back() {
        this.location.back();
    }

    register() {
        this.registerService.register(this.event.eventId).subscribe(x => {
            this.location.back();
        });
    }
}