import { Inject, Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IEvent } from '../models/event.model';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class EventService {
    private BASE_URL: string;
    private SERVICE_URL: string = "/EventManagement/Resources/api/Event";

    constructor(private http: Http) {
    }

    getAll(): Observable<IEvent[]> {
        return this.http
            .get(`${this.SERVICE_URL}`)
            .map((response) => {
                var json = response.json();
                for (let ev of json) {
                    ev["start"] = new Date(ev["start"]);
                    ev["end"] = new Date(ev["end"]);
                }
                return json;
            });
    
    }

    get(id: number): Observable<IEvent> {
        console.log("get service");
        return this.http
            .get(`${this.SERVICE_URL}/${id}`)
            .map((response) => response.json());
    }

    getByResource(resourceId: number): Observable<IEvent[]> {
        return this.http
            .get(`${this.SERVICE_URL}/byResource/${resourceId}`)
            .map((response) => response.json());
    }

    create(ev: IEvent): Observable<IEvent> {
        let headers = new Headers();
        headers.append('Accept', 'application/json');
        headers.append('Content-Type', 'application/json');
        return this.http.post(`${this.SERVICE_URL}`, JSON.stringify(ev), { headers: headers })
            .map(res => res.json());
    }

    remove(id: number): Observable<IEvent> {
        return this.http
            .delete(`${this.SERVICE_URL}/${id}`)
            .map((response) => response.json());
    }
}