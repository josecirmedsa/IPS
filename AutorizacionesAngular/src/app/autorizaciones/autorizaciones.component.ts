import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Autorizar, Prestador, Elegibilidad, Afiliado, Search, Prestacion, Authorize, practica, Authorizeresponse } from '../_models';
import { Router } from "@angular/router";
import { AuthenticationService, AutorizacionService } from '../_services';
import { ObraSocial } from '../_models/ObraSocial';

import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { NgbDate, NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: 'autorizaciones.component.html'

   
})

export class AutorizacionesComponent implements OnInit {

    autorizar = Autorizar;
    authResponse = new Authorizeresponse;

    model: NgbDateStruct;
    date: { year: number, month: number };

    hoveredDate: NgbDate;

    fromDate: NgbDate;
    toDate: NgbDate;

    constructor(
        private autorizacionService: AutorizacionService,
        private authenticationService: AuthenticationService,
        private router: Router,
        private modalService: NgbModal,
        private calendar: NgbCalendar

    ) {
        this.fromDate = calendar.getToday();
        this.toDate = calendar.getNext(calendar.getToday(), 'd', 10);
    }

    ngOnInit(): void {
       
    }

    selectToday() {

        this.model = this.calendar.getToday();
        console.log(this.model);
    }
   

}