import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Autorizar, Prestador, Elegibilidad, Afiliado, Search, Prestacion, Authorize, practica, Authorizeresponse } from '../_models';
import { Router } from "@angular/router";
import { AuthenticationService, AutorizacionService } from '../_services';
import { ObraSocial } from '../_models/ObraSocial';

import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({ templateUrl: 'autorizar.component.html' })

export class AutorizarComponent implements OnInit {

    autorizar = Autorizar;
    authResponse = new Authorizeresponse;
   
    prestadores: Prestador[] = [];
    facturadores: Prestador[] = [];
    matriculas: Prestador[] = [];
    prestaciones: Prestacion[] = [];

    OS: ObraSocial[] = [];
    OSXIdPre: ObraSocial[] = [];

    prestador: number;
    facturador: number;
    obrasocial: number;
    AfilNm: string;
    AfilOk: number;
    logo: string;
    carnetMask: string = "Credencial Nr.";
    codigo: string = "";
    codigoInexistente: number; 
    descripcion: string = "";
    cantidad: number=1;

    msjPrestador = "";

    exists: boolean;
    prestacionError: string;

    closeResult: string;

    alerta: string = "Kapanga´s Rule's";

    //****************************************** AutoComplete ******************************************
    keyword = 'descripcion';
    practicas: any[] = [];
    desc: string = "";
    data: any[] = [];

    keywordCarnet = 'descripcion';
    practicasCarnet: any[] = [];
    autoCarnet: string = "";
    dataCarnet: any[] = [];


    constructor(
        private autorizacionService: AutorizacionService,
        private authenticationService: AuthenticationService,
        private router: Router,
        private modalService: NgbModal
    ) {
       
    }

    /*************************************************************************************************** */
    /*********************************************** INIT ********************************************** */
    /*************************************************************************************************** */

    ngOnInit(): void {
        this.msjPrestador = "Prestador";

        this.getOS();
        this.getPrestadores();
    }

    open(content: any) {
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
            console.log('save');
            this.closeResult = `Closed with: ${result}`;
        }, (reason) => {
            console.log('cruz');
            this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
    }

    private getDismissReason(reason: any): string {
        if (reason === ModalDismissReasons.ESC) {
            return 'by pressing ESC';
        } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
            return 'by clicking on a backdrop';
        } else {
            return `with: ${reason}`;
        }
    }

    /*************************************************************************************************** */
    /****************************************** INIT Functions ***************************************** */
    /*************************************************************************************************** */

    getPrestadores(): void {
        this.autorizacionService.getAll().pipe(first()).subscribe(items => {

            let pres: Prestador[] = [];
            let fact: Prestador[] = [];
            let p: Prestador;

            this.matriculas = items;
            if (items != null) {
                if (items.length == 1) {
                    pres = items;
                    fact = items;
                    p = items[0];
                }
                else {
                    items.forEach(function (value) {
                        if (value.type == 0) {
                            pres.push(value);
                            if (pres.length == 1) { fact.push(value); }
                            if (p == null) { p = value; }
                        }
                        else {
                            fact.push(value);
                        }
                    });
                }

                this.facturadores = fact;
                this.prestadores = pres;
                this.prestador = Number(p.id);
                this.facturador = Number(p.id);
                console.log(fact, pres, p.id );
            }
            else {
                this.authenticationService.logout();
                this.router.navigate(['/login']);
            }
        });
    }

    getOS(): void {
        this.autorizacionService.getOS().pipe(first()).subscribe(items => {
            console.log(items);
            this.OS = items;
        });
    }
    /*************************************************************************************************** */
    /********************************************** Change ********************************************* */
    /*************************************************************************************************** */
    presChange(): void {
        
        if (this.matriculas.length > 1) {
            let pres: number = this.prestador;
            let fac: Prestador[] = [];
            this.facturadores = [];
            this.matriculas.forEach(function (value) {
                if (value.type == 1 || value.id == Number(pres)) {
                    fac.push(value);
                }
            });
            this.facturadores = fac;
            this.prestador = pres;
        }
    }

    osChange(): void {
        //console.log('change', this.obrasocial);
        console.log('obrasocial', this.obrasocial);
        switch (Number(this.obrasocial)) {
            case 1: //SwissMedical "xxxxxxxxxxxxx - Omitir 800006"
                this.logo = "swiss";
                this.carnetMask = "Omitir 800006";
                console.log('swis');
                break;
            case 2://Aca Salud
                this.logo = "aca";
                this.carnetMask = "xxxxxxx - Nº Credencial";
                break;
            case 3: //???
                break;
            case 4: //??
                break;
            case 5: //Boreal
            case 8:
                this.logo = "boreal";
                this.carnetMask = "xxxxxxxx/x - Nº Credencial/Nº G. Familiar";
                break;
            case 6: //Medife
                this.logo = "medife";
                this.carnetMask = "xxxxxxx - Nº Credencial";
                break;
            case 7: //Red de Seguro
                this.logo = "red";
                this.carnetMask = "xxxxxxxx/x - Nº Credencial/Nº Adicional";
                break;
            case 8:
                this.logo = "";
                //vm.image = "http://localhost:13582/images/filmhunterlogo.png";
                break;
            case 9:
                this.logo = "sancor";
                this.carnetMask = "xxxxxx/xx - Nº Credencial/Nº Adicional";
                break;
            case 10:
                this.logo = "luzyfuerza";
                this.carnetMask = "xx-xxxxx-x/xx - Nº Credencial";
                break;
        }
        //reset values 
        this.autoCarnet = "";
        this.AfilNm = "";
        this.AfilOk = 1;

    }

     /*************************************************************************************************** */
    /*********************************************** Focus ********************************************** */
    /*************************************************************************************************** */
    osFocus(): void {

        let pres: number = this.prestador;
        let list:  ObraSocial[] = [];
        this.OSXIdPre = [];
        this.OS.forEach(function (value) {
            if (value.idpre == Number(pres)) {
                list.push(value);
            }
        });
        this.OSXIdPre = list;
    }

    codigoFocusOut(el: any, el1:any): void {

        if (this.codigo.length == 6) {
            let search: Search = new Search();
            search.cod = this.codigo;
            search.OsId = this.obrasocial.toString();
            this.codigoInexistente = 0;
            this.autorizacionService.getPractDesc(search).pipe(first()).subscribe(items => {
                this.desc = "(" + items.cod + ") - " + items.descripcion;
                if (items.descripcion.indexOf("Código Inexistente") >= 0) {
                    this.codigoInexistente = 1;
                }
                else {
                    el.focus();
                }
            });
        }
        else {
            if (this.codigo.length > 1) {
                this.alerta = "El código de practica debe tener 6 digitos. por favor verifique";
                let element: HTMLElement = document.getElementById('message');
                element.click();
            }
        }
    }
   
    /*************************************************************************************************** */
    /*********************************************** Click ********************************************* */
    /*************************************************************************************************** */


    getElegibilidad(): void {
        this.AfilOk = 1;
        this.AfilNm = "";
        if (!(this.autoCarnet == undefined || this.autoCarnet == "") && this.obrasocial != undefined) {
            let elegibilidad = new Elegibilidad();
            elegibilidad.Credencial = this.autoCarnet;
            elegibilidad.OsId = this.obrasocial;
            elegibilidad.IdPre = this.prestador.toString();

            this.autorizacionService.getElegibilidad(elegibilidad).pipe(first()).subscribe(items => {
                let afil = new Afiliado();
                afil = items;

                if (afil.name == null) {
                    this.AfilNm = "Afiliado inexistente o inhabilitado";
                    this.AfilOk = 1;
                }
                else {
                    if (afil.name.trim() === "Afiliado inexistente o inhabilitado") {
                        this.AfilNm = afil['name'];
                        this.AfilOk = 1;
                    } else {
                        this.AfilNm = afil['name'] + " (" + afil['plan'] + ")";
                        this.AfilOk = 0;
                    }
                }
            });
        }
    }

    prestacionAddClick(): void {

        const that = this;
        this.prestacionError = "";
        if ((this.codigo != undefined && this.codigo.length > 0) &&
            (this.cantidad != undefined && this.cantidad > 0)) {

            var codigo = this.codigo;
            var exists = false;
            this.prestaciones.forEach(function (val) {
                if (val.codPres == codigo) {
                    exists = true;
                    that.alerta = "El código de practica ya existe!";
                    let element: HTMLElement = document.getElementById('message');
                    element.click();
                }
            });
            
            if (this.desc.indexOf("Inexistente") > 0) {
                exists = true;
                that.alerta = "El código de practica ya existe!";
                let element: HTMLElement = document.getElementById('message');
                element.click();
            }

            if (!exists) {
                var pres: Prestacion = {
                    codPres: codigo,
                    descripcion: this.desc,
                    cant: this.cantidad,
                    action: "Eliminar"
                }

                this.prestaciones.push(pres);

                this.codigo = "";
                this.desc = "";
                this.cantidad = 1;
            }
        }
        console.log(this.prestaciones);
    }

    deleteItem(cod: string): void {
        console.log(cod);
        this.prestaciones = this.prestaciones.filter(item => item.codPres !== cod);
    }

    autorizarClick(): void {
        let auth: Authorize = new Authorize();
        auth.OsId = this.obrasocial;
        auth.PrestadorId = this.prestador.toString();
        auth.FacturadorId = this.facturador.toString();

        if (typeof this.autoCarnet == "object") {
            auth.Credencial = this.autoCarnet.descripcion;
        }
        else {
            auth.Credencial = this.autoCarnet;
        }

        

        console.log('carnet', this.autoCarnet)

        let pracicas: practica[] = [];

       

        this.prestaciones.forEach(function (p) {
            let prac: practica = new practica;
            prac.codPres = p.codPres;
            prac.cant = p.cant;

            pracicas.push(prac); 
        });

        auth.Prestaciones = pracicas;

        console.log(auth);

        this.autorizacionService.authorize(auth).pipe(first()).subscribe(items => {
            //  this.descripcion = items;
            console.log('Autorizacion', items);
            this.authResponse = items;

            let element: HTMLElement = document.getElementById('autorizacion');
            element.click();

        });
    }

/*************************************************************************************************** */
/********************************************* KeyPress ******************************************** */
/*************************************************************************************************** */
    codigoKeyPress(): void {
        console.log(this.codigo, this.codigo.length);
        if (this.codigo.length > 2) {
            //ir a la base a buscar el cod
            if (this.codigo.length == 3) {
                let search: Search = new Search();
                search.cod = this.codigo;
                search.OsId = this.obrasocial.toString();

                this.autorizacionService.getPractDesc(search).pipe(first()).subscribe(items => {
                    //  this.descripcion = items;
                });
            } else {
                //Busqueda local
            }
        }
        else {
            this.practicas = [];
            this.data = [];
        }
    }

/*************************************************************************************************** */
/*********************************************** Ng-If ********************************************* */
/*************************************************************************************************** */

    showAutorizar(): boolean {
        if (this.prestaciones.length > 0) {
            return true;
        } else {
            return false;
        }
    }

    ckeckInexistente(): boolean {
        return this.AfilOk === 0;
    }

    ckeckDescripcion(): boolean {
        return this.codigoInexistente === 0;
    }
/*************************************************************************************************** */
/******************************************* AutoComplete ****************************************** */
/*************************************************************************************************** */

    getPrestaciones(): void {

        let search: Search = new Search();
        search.cod = this.desc;
        search.OsId = this.obrasocial.toString();

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
        else {
            this.data = [];
            this.practicas = [];
        }
    }

    itemclick(e: Event, cod: number, el:any): void {
        el.focus();
    }

    selectEvent(item: any, el: any, el1: any) {
         this.codigo = item.cod;
        this.desc = item.descripcion;
        el.focus();
        el1.focus();
    }

    onChangeSearch(val: string) {
        // fetch remote data from here
        // And reassign the 'data' which is binded to 'data' property.
    }

    onFocused() {
        // do something when input is focused
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    getPrestacionesCarnet(): void {
        console.log('getPrestacionesCarnet');

       if (this.autoCarnet.length > 1) {
            if (this.autoCarnet.length == 2) {

                let search: Search = new Search();
                search.prestadorId = this.prestador;
                search.OsId = this.obrasocial.toString();
                search.cod = this.autoCarnet;
                console.log(search);

                this.autorizacionService.getCarnetList(search).pipe(first()).subscribe(items => {
                    console.log(items);
                    this.dataCarnet = items;
                    this.practicasCarnet = items;
                });
            }
            else {
                let x = this.practicasCarnet.filter(a => a.descripcion.indexOf(this.desc.toUpperCase()) > -1);
                this.dataCarnet = x;
            }
        }
        else {
            this.dataCarnet = [];
            this.practicasCarnet = [];
        }
    }

    itemclickCarnet(e: Event, cod: number, el: any): void {
        //console.log('click description', e, cod);
        //el.focus();
    }

    selectEventCarnet(item: any, el: any) {
        this.autoCarnet = item.descripcion;
        let element: HTMLElement = document.getElementById('buscarAfil');
        element.click();
    }

}