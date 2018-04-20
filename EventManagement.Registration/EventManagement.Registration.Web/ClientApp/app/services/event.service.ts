import { Inject, Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IEvent } from '../models/event.model';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class EventService {
    private BASE_URL: string;
    private SERVICE_URL: string = "/EventManagement/Registration/api/Event";

    constructor(private http: Http) {
    }

    getAll(): Observable<IEvent[]> {
        return this.http
            .get(`${this.SERVICE_URL}`)
            .map((response) => {
                return response.json();
            });
    }

    get(id: number): Observable<IEvent> {
        return this.http
            .get(`${this.SERVICE_URL}/${id}`)
            .map((response) => response.json());
    }
}