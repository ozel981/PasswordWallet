import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SignInData } from '../contracts/signInData';
import { CreatePasswordCommand } from '../passwords/password-wallet/commands/createPasswordCommand';
import { UpdatePasswordCommand } from '../passwords/password-wallet/commands/updatePasswordCommand';
import { Password } from '../passwords/contracts/password';
import { APIURL, AUTHSTR } from '../settings';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  fetchPasswords(): Observable<Password[]> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.get<Password[]>(APIURL + 'Password', { headers });
  }

  fetchPassword(passwordId: number): Observable<Password> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.get<Password>(APIURL + 'Password/' + passwordId, { headers });
  }

  createPassword(command: CreatePasswordCommand): Observable<Password> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.post<Password>(APIURL + 'Password', command, { headers });
  }

  updatePassword(passwordId: number, command: UpdatePasswordCommand): Observable<Password> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    return this.http.put<Password>(APIURL + 'Password/' + passwordId, command, { headers });
  }

  deletePassword(passwordId: number): Observable<any> {

    var headers = { 'Authorization': 'Basic ' + localStorage.getItem(AUTHSTR) };

    let https = APIURL + 'Password/' + passwordId;

    return this.http.delete(https, { headers });
  }
}
