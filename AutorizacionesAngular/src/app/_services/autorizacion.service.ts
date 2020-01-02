import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User, Prestador, Elegibilidad, Afiliado, Search, Carnetsearch, Authorize, Authorizeresponse } from '../_models';
import { ObraSocial} from '../_models/ObraSocial';

@Injectable()
export class AutorizacionService {
    constructor(private http: HttpClient) { }


    getAll() {
        return this.http.get<Prestador[]>(`${config.apiUrl}/api/Autorizacion/Prestadores`);
    }

    getOS() {
        return this.http.get<ObraSocial[]>(`${config.apiUrl}/api/Autorizacion/GetObrasSociales`);
    }

    getElegibilidad(model: Elegibilidad) {
        console.log(model);
        return this.http.post<Afiliado>(`${config.apiUrl}/api/Autorizacion/GetElegibilidad`, model );
    }

    getPractDesc(model: Search) {
        return this.http.post<Search>(`${config.apiUrl}/api/Autorizacion/GetPractDesc`, model);
    }

    getPractDescList(model: Search) {
        console.log(model);
        return this.http.post<Search[]>(`${config.apiUrl}/api/Autorizacion/GetPractDescList`, model);
    }

    getCarnetList(model: Search) {
        console.log(model);
        return this.http.post<Carnetsearch[]>(`${config.apiUrl}/api/Autorizacion/GetCarnetList`, model);
    }

    authorize(model: Authorize) {
        console.log('Service', model);
        return this.http.post<Authorizeresponse>(`${config.apiUrl}/api/Autorizacion/Authorize`, model);
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
    //    return this.http.get<User[]>(`${config.apiUrl}/api/Autorizacion`);
    //}

    //getPrestadores() {
    //    return this.http.post(`${config.apiUrl}/api/Autorizacion`,null);
    //}

    //elegibilidad(id: number) {
    //    return this.http.get(`${config.apiUrl}/api/Autorizacion/Elegibilidad` + id);
    //}

    //register(user: User) {
    //    return this.http.post(`${config.apiUrl}/api/Autorizacion/register`, user);
    //}

    //update(user: User) {
    //    return this.http.put(`${config.apiUrl}/api/Autorizacion/` + user.id, user);
    //}

    //delete(id: number) {
    //    return this.http.delete(`${config.apiUrl}/api/Autorizacion/` + id);
    //}
}