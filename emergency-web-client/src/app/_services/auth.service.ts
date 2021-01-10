import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {
  }

  checkAuthenticated() {
  }

  login(username, password): Observable<any> {

    return this.http.post(environment.gatewayUrl + '/authenticate', {
      username,
      password
    }, httpOptions);
  }
}
