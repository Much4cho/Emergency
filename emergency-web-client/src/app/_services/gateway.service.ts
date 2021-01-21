import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Emergency } from '../_model/Emergency';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class GatewayService {

  constructor(private http: HttpClient) {

  }

  addEmergency(emergency: Emergency): Observable<Emergency> {
    return this.http.post<Emergency>(environment.gatewayUrl + '/client/Emergencies', emergency);
  }

  getEmergencies(): Observable<Array<Emergency>> {
    return this.http.get<Array<Emergency>>(environment.gatewayUrl + '/emergencies');
  }

  updateEmergency(emergency: Emergency): Observable<Emergency> {
    return this.http.put<Emergency>(environment.gatewayUrl + '/emergencies', emergency);
  }

  getTeams(): Observable<any> {
    return this.http.get<any>(environment.gatewayUrl + '/teams');
  }

  updateTeam(team): Observable<any> {
    console.log(team);
    return this.http.put<any>(environment.gatewayUrl + '/teams', team);
  }

  getTeamsEmergency(teamId): Observable<any> {
    return this.http.get<any>(environment.gatewayUrl + '/teamEmergency/' + teamId);
  }

  getEmergencyStatus(identifier): Observable<any> {
    return this.http.get<any>(environment.gatewayUrl + '/emergency/' + identifier);
  }
}
