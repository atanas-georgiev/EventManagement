import { Inject, Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IResource } from '../models/resource.model';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class ResourceService {
    private BASE_URL: string;
    private SERVICE_URL: string = "/EventManagement/Resources/api/Resource";

    constructor(private http: Http) {
    }

    getAll(): Observable<IResource[]> {
        return this.http
            .get(`${this.SERVICE_URL}`)
            .map((response) => {
                return response.json();
            });
    }

    get(id: number): Observable<IResource> {
        return this.http
            .get(`${this.SERVICE_URL}/${id}`)
            .map((response) => response.json());
    }

    create(res: IResource): Observable<IResource> {
        let headers = new Headers();
        headers.append('Accept', 'application/json');
        headers.append('Content-Type', 'application/json');
        return this.http.post(`${this.SERVICE_URL}`, JSON.stringify(res), { headers: headers })
            .map(res => {
                console.log(res);
                return res.json();
            });
    }

    remove(id: number): Observable<IResource> {
        return this.http
            .delete(`${this.SERVICE_URL}/${id}`)
            .map((response) => response.json());
    }
}