import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Password } from '../passwords/contracts/password';
import { SharePasswordCommand } from '../passwords/shared-password-wallet/commands/sharePasswordCommand';
import { APIURL, AUTHSTR } from '../settings';

@Injectable({
  providedIn: 'root'
})
export class SharedPasswordService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  fetchPasswords(): Observable<Password[]> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.get<Password[]>(APIURL + 'SharedPassword', { headers });
  }

  fetchPassword(passwordId: number): Observable<Password> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.get<Password>(APIURL + 'SharedPassword/' + passwordId, { headers });
  }

  sharePassword(command: SharePasswordCommand): Observable<Password> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.post<Password>(APIURL + 'SharedPassword', command, { headers });
  }

  deletePassword(passwordId: number): Observable<any> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    let https = APIURL + 'SharedPassword/' + passwordId;

    return this.http.delete(https, { headers });
  }
}
