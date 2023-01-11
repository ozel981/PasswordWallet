import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SignInData } from 'src/app/contracts/signInData';
import { PasswordService } from 'src/app/repositories/password.service';
import { Password } from '../../contracts/password';

@Component({
  selector: 'password-details',
  templateUrl: './password-details.component.html',
  styleUrls: ['./password-details.component.css']
})
export class PasswordDetailsComponent implements OnInit {

  encryptedPassword: string = '';
  decryptedPassword?: string = undefined;
  isPasswordEncrypted: boolean = true;

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

  @Output() deletePasswordEvent = new EventEmitter<Password>();

  constructor(private passwordService: PasswordService) { }

  ngOnInit(): void {
    this.encryptedPassword = this.password.passwordText;
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
    this.passwordService.fetchPassword(this.password.id).subscribe(password => {
      this.password.passwordText = password.passwordText;
      this.decryptedPassword = password.passwordText;
      this.isPasswordEncrypted = false;
    });
  }

  deletePassword() {
    this.passwordService.deletePassword(this.password.id).subscribe(result => {
      this.deletePasswordEvent.emit(this.password);
    });
  }
}
