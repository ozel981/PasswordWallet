import { Component, Input, OnInit } from '@angular/core';
import { SignInData } from '../../contracts/signInData';
import { PasswordService } from '../../repositories/password.service';
import { CreatePasswordCommand } from './commands/createPasswordCommand';
import { Password } from '../contracts/password';

@Component({
  selector: 'password-wallet',
  templateUrl: './password-wallet.component.html',
  styleUrls: ['./password-wallet.component.css']
})
export class PasswordWalletComponent implements OnInit {

  selectedPasswordId?: number;

  createPasswordCommand: CreatePasswordCommand = {
    passwordText: '',
    webAdderss: '',
    description: '',
    login: ''
  }

  passwords: Password[] = [];

  signInData: SignInData = {
    login: '',
    password: ''
  }

  @Input() login: string = '';

  constructor(private passwordService: PasswordService) { }

  ngOnInit(): void {
    this.signInData.login = this.login;
    this.passwordService.fetchPasswords().subscribe(passwords => this.passwords = passwords);
  }

  createPassword() {
    this.passwordService.createPassword(this.createPasswordCommand).subscribe(password => {
      this.passwords.push(password);
    })
  }

  deletePassword(password: Password) {
    let index = this.passwords.indexOf(password);
    this.passwords.splice(index, 1);
  }
}
