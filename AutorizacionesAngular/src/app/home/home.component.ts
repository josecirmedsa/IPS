import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router } from "@angular/router";

import { User, Search } from '../_models';
import { UserService, AuthenticationService, AutorizacionService } from '../_services';


@Component({templateUrl: 'home.component.html'})
export class HomeComponent implements OnInit {
    currentUser: User;

    users: User[] = [];

    desc: string = "";
    data: any[] = [];

    practicas: any[] =[];

    constructor(private userService: UserService, private authenticationService: AuthenticationService, private router: Router, private autorizacionService: AutorizacionService ) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        console.log(this.currentUser);
    }

    ngOnInit() {
        this.loadAllUsers();
    }

    deleteUser(id: number) {
        this.userService.delete(id).pipe(first()).subscribe(() => { 
            this.loadAllUsers() 
        });
    }

    private loadAllUsers() {
        this.userService.getAll().pipe(first()).subscribe(users => { 
            console.log(users)
            if (users != null) {
                this.users = users;
            }
            else {
                this.authenticationService.logout();
                this.router.navigate(['/login']);
            }
        });
    }

    keyword = 'descripcion';
    
    getPrestadores(): void {

        let search: Search = new Search();
        search.cod = this.desc;
        search.OsId = "2";

        if (this.desc.length > 2) {
            if (this.desc.length == 3) {
                this.autorizacionService.getPractDescList(search).pipe(first()).subscribe(items => {
                    this.data = items;
                    this.practicas = items;
                });
            }
            else {
                
                let x = this.practicas.filter(a => a.descripcion.indexOf(this.desc.toUpperCase()) > -1);
                
                this.data = x;
            }
        }
    }

    itemclick(e: Event): void {
        console.log(e);
    }

    selectEvent(item:any) {
        // do something with selected item
        console.log(item);
    }

    onChangeSearch(val: string) {
        // fetch remote data from here
        // And reassign the 'data' which is binded to 'data' property.
    }

    onFocused() {
        // do something when input is focused
    }
}