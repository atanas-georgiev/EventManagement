import { Component, OnInit } from '@angular/core';
import { IEvent } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { ResourceService } from "../../services/resource.service";
import { ActivatedRoute } from '@angular/router';
import { IResource } from "../../models/resource.model";

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {

    event: IEvent;
    resource: IResource;

    constructor(private eventService: EventService, private resourceService: ResourceService, private route: ActivatedRoute) { }

  ngOnInit() {
      this.route.params.subscribe(params => {
          let currId: number = params['id'];
          this.eventService.get(currId).subscribe(e => {
              this.event = e;
              console.log(e.resourceId);
              this.resourceService.get(this.event.resourceId).subscribe(r => {
                  this.resource = r;
                  console.log(r);
                  console.log(this.resource);
              });
          });
      });
  }

}
