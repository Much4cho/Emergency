import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Emergency} from '../_model/Emergency';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class GatewayService {

  constructor(private http: HttpClient) {
  }

  addEmergency(emergency: Emergency): Observable<Emergency> {
    return this.http.post<Emergency>(environment.gatewayUrl + '/client/Emergencies', emergency, httpOptions);
  }

}
