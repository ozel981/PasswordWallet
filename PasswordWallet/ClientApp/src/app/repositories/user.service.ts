import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UpdateUserCommand } from '../commands/createUserCommand';
import { SignInData } from '../contracts/signInData';
import { APIURL, AUTHSTR } from '../settings';
import { User } from '../sign-in/contracts/user';
import { CreateUserCommand } from '../sign-up/commands/createUserCommand';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAuthStr(signInData: SignInData): string {
    let data = signInData.login + ':' + signInData.password;

    var Buffer = require('buffer/').Buffer;

    return Buffer.from(data).toString('base64');
  }

  signIn(signInData: SignInData): Observable<User> {
    var headers = { 'Authorization': 'Basic ' + this.getAuthStr(signInData) };

    return this.http.post<User>(APIURL + 'User/signIn', { login: signInData.login }, { headers });
  }

  createUser(command: CreateUserCommand): Observable<number> {
    var Buffer = require('buffer/').Buffer;

    return this.http.post<number>(APIURL + 'User', command);
  }

  changePassword(signInData: SignInData, command: UpdateUserCommand) {
    var headers = { 'Authorization': 'Basic ' + this.getAuthStr(signInData) };

    return this.http.put<number>(APIURL + 'User/changePassword', command, { headers });
  }
}
