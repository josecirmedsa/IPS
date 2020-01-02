import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User } from '../_models';

@Injectable()
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${config.apiUrl}/api/Autorizacion/Prestadores`);
    }

    getById(id: number) {
        return this.http.get(`${config.apiUrl}/api/Autorizacion` + id);
    }

    register(user: User) {
        return this.http.post(`${config.apiUrl}/api/Autorizacion/register`, user);
    }

    update(user: User) {
        return this.http.put(`${config.apiUrl}/api/Autorizacion/` + user.id, user);
    }

    delete(id: number) {
        return this.http.delete(`${config.apiUrl}/api/Autorizacion/` + id);
    }

    //getAll() {
    //    return this.http.get<User[]>(`${config.apiUrl}/api/Values`);
    //}

    //getById(id: number) {
    //    return this.http.get(`${config.apiUrl}/api/Values` + id);
    //}

    //register(user: User) {
    //    return this.http.post(`${config.apiUrl}/api/Values/register`, user);
    //}

    //update(user: User) {
    //    return this.http.put(`${config.apiUrl}/api/Values/` + user.id, user);
    //}

    //delete(id: number) {
    //    return this.http.delete(`${config.apiUrl}/api/Values/` + id);
    //}
}