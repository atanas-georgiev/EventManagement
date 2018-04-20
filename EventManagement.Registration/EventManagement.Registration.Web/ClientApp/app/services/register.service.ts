import { IRegistrationUser } from './../models/registrationuser.mode';
import { Registration } from './../models/registration.model';
import { Inject, Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IEvent } from '../models/event.model';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class RegisterService {
    private BASE_URL: string;
    private SERVICE_URL: string = "/EventManagement/Registration/api/Registration";

    constructor(private http: Http) {
    }

    register(eventId: number): Observable<any> {
        let headers = new Headers();
        let postData = new Registration();
        postData.eventId = eventId;

        headers.append('Content-Type', 'application/json');
        return this.http
            .post(`${this.SERVICE_URL}`,
            postData,
            { headers })
            .map(res => res.json());
    }

    get(id: number): Observable<IRegistrationUser[]> {
        return this.http
        .get(`${this.SERVICE_URL}/${id}`)
        .map((response) => {
            return response.json();
        });
    }
}