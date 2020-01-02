import { Component } from '@angular/core';

import { User } from '../app/_models';
import { UserService } from '../app/_services';

@Component({
    selector: 'app',
    templateUrl: 'app.component.html'
})

export class AppComponent {
    currentUser: User;
    users: User[] = [];

    constructor(private userService: UserService) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    checkUser(): boolean{
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        return this.currentUser != null;
    }
}