import { Component, OnInit, Input } from '@angular/core';
import { IResource } from "../../models/resource.model";
import { FormGroup, FormControl, FormBuilder } from "@angular/forms";
import { ResourceService } from "../../services/resource.service";

@Component({
  selector: 'app-resource-add',
  templateUrl: './resource-add.component.html',
  styleUrls: ['./resource-add.component.css']
})
export class ResourceAddComponent implements OnInit {
    @Input() resource: IResource;

    resourceForm = new FormGroup({
        name: new FormControl(),
        location: new FormControl(),
        placesCount: new FormControl(),
        rent: new FormControl()
    });

    constructor(private fb: FormBuilder, private resourceService: ResourceService) {
    }

    ngOnInit() {
    }

    onSubmit() {
        if (!this.resourceForm.valid) {
            for (let controlName in this.resourceForm.controls) {
                if (!this.resourceForm.controls[controlName].valid) {
                    this.resourceForm.controls[controlName].markAsDirty();
                }
            }
            return;
        } 

        if (confirm("Are you sure?")) {
            let resource = <IResource>({
                name: this.resourceForm.value["name"],
                location: this.resourceForm.value["location"],
                placesCount: this.resourceForm.value["placesCount"] || 0,
                rent: this.resourceForm.value["rent"] || 0
            });
            this.resourceService.create(resource).subscribe(
                data => {
                    this.resource = data as IResource;
                    this.resourceForm.reset();
                },
                e => console.log('An error occured: ', e)
            );
        }
    }

}
