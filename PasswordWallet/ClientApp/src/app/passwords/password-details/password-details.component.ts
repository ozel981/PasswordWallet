import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SignInData } from 'src/app/contracts/signInData';
import { PasswordService } from 'src/app/repositories/password.service';
import { SharedPasswordService } from 'src/app/repositories/shared-password.service';
import { Password } from '../contracts/password';
import { SharePasswordCommand } from '../shared-password-wallet/commands/sharePasswordCommand';

@Component({
  selector: 'password-details',
  templateUrl: './password-details.component.html',
  styleUrls: ['./password-details.component.css']
})
export class PasswordDetailsComponent implements OnInit {

  encryptedPassword: string = '';
  decryptedPassword?: string = undefined;
  isPasswordEncrypted: boolean = true;

  sharePasswordCommand: SharePasswordCommand = {
    passwordId: -1,
    userLogin: '',
  }

  @Input() password: Password = {
    id: -1,
    passwordText: '',
    webAdderss: '',
    description: '',
    login: ''
  }

  @Input() signInData: SignInData = {
    login: '',
    password: ''
  }

  @Input() isSharedPassword = false;

  @Output() deletePasswordEvent = new EventEmitter<Password>();

  constructor(private passwordService: PasswordService, private sharedPasswordService: SharedPasswordService) { }

  ngOnInit(): void {
    this.encryptedPassword = this.password.passwordText;
    this.sharePasswordCommand.passwordId = this.password.id;
  }

  switchPasswordEncryption() {
    if (this.isPasswordEncrypted) {
      if (this.decryptedPassword !== undefined) {
        this.password.passwordText = this.decryptedPassword;
        this.isPasswordEncrypted = !this.isPasswordEncrypted;
      } else {
        this.fetchPassword();
      }
    } else {
      this.password.passwordText = this.encryptedPassword;
      this.isPasswordEncrypted = !this.isPasswordEncrypted;
    }
  }

  fetchPassword() {
    if (this.isSharedPassword) {
      this.sharedPasswordService.fetchPassword(this.password.id).subscribe(password => {
        this.password.passwordText = password.passwordText;
        this.decryptedPassword = password.passwordText;
        this.isPasswordEncrypted = false;
      });
    } else {
      this.passwordService.fetchPassword(this.password.id).subscribe(password => {
        this.password.passwordText = password.passwordText;
        this.decryptedPassword = password.passwordText;
        this.isPasswordEncrypted = false;
      });
    }
  }

  deletePassword() {
    if (this.isSharedPassword) {
      this.sharedPasswordService.deletePassword(this.password.id).subscribe(result => {
        this.deletePasswordEvent.emit(this.password);
      });
    } else {
      this.passwordService.deletePassword(this.password.id).subscribe(result => {
        this.deletePasswordEvent.emit(this.password);
      });
    }
  }

  sharePassword() {
    this.sharedPasswordService.sharePassword(this.sharePasswordCommand).subscribe(result => {
      alert("Password shared");
    });
  }
}
