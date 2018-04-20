import { Inject, Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class UserService {
    private BASE_URL: string;
    private SERVICE_URL: string = "/EventManagement/Portal/api/User";

    constructor(private http: Http) {
    }

    get(): Observable<string> {
        return this.http
            .get(`${this.SERVICE_URL}`)
            .map((response) => response.json());
    }

    isAdmin(): Observable<boolean> {
        return this.http
            .get(`${this.SERVICE_URL}/IsAdmin`)
            .map((response) => response.json());
    }
}