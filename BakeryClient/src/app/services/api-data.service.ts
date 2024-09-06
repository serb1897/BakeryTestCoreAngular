import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface IApiResultObject {
    status: boolean;
    message: string;
    data: any;
}

@Injectable()
export class ApiDataService {

    private defaultHeaders: HttpHeaders;

    constructor(private http: HttpClient) {
        this.defaultHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
            'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token'
        });
    }

    public Post(url: string, body: any) {
        return this.http.post(url, body, {
            headers: this.defaultHeaders,
            withCredentials: false
        });
    }

    public Get(url: string) {
        return this.http.get(url, {
            headers: this.defaultHeaders,
            withCredentials: false
        });
    }

    public Patch(url: string, body: any) {
        return this.http.patch(url, body, {
            headers: this.defaultHeaders,
            withCredentials: false
        });
    }

    public Put(url: string, body: any) {
        return this.http.put(url, body, {
            headers: this.defaultHeaders,
            withCredentials: false
        });
    }

    public Delete(url: string) {
        return this.http.delete(url, {
            headers: this.defaultHeaders,
            withCredentials: false
        });
    }
}