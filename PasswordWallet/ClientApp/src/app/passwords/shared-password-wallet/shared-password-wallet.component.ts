import { Component, Input, OnInit } from '@angular/core';
import { SignInData } from 'src/app/contracts/signInData';
import { SharedPasswordService } from 'src/app/repositories/shared-password.service';
import { Password } from '../contracts/password';
import { SharePasswordCommand } from './commands/sharePasswordCommand';

@Component({
  selector: 'shared-password-wallet',
  templateUrl: './shared-password-wallet.component.html',
  styleUrls: ['./shared-password-wallet.component.css']
})
export class SharedPasswordWalletComponent implements OnInit {

  selectedPasswordId?: number;

  passwords: Password[] = [];

  signInData: SignInData = {
    login: '',
    password: ''
  }

  @Input() login: string = '';

  constructor(private passwordService: SharedPasswordService) { }

  ngOnInit(): void {
    this.signInData.login = this.login;
    this.passwordService.fetchPasswords().subscribe(passwords => this.passwords = passwords);
  }

  deletePassword(password: Password) {
    let index = this.passwords.indexOf(password);
    this.passwords.splice(index, 1);
  }
}
