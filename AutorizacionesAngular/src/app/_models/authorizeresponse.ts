//import { practica } from ".";

export class Authorizeresponse {
    //OsId: number;
    //PrestadorId: string;
    //FacturadorId: string;
    //Credencial: string;
    //Prestaciones: practica[];

    id: number; 
    authNr: string;
    fecha: string;
    afiliado: string;
    plan: string; 
    iva: string;
    identificacionNro: string;
    aseguradora: string;
    matricula: string;
    profesional: string;
    //List < AutorizacionVerDet > Detalle 
    authNrAnulacion: string;
    estado: string;
}

