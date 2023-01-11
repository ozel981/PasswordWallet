import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../repositories/user.service';
import { AUTHSTR } from '../settings';
import { CreateUserCommand } from './commands/createUserCommand';

@Component({
  selector: 'sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  createUserCommand: CreateUserCommand = {
    login: "",
    password: "",
    isPasswordKeptAsHash: false
  }

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
  }

  signup() {
    this.userService.createUser(this.createUserCommand).subscribe(userId => {
      let encodedAuthStr = this.userService.getAuthStr({ login: this.createUserCommand.login, password: this.createUserCommand.password })
      localStorage.setItem(AUTHSTR, encodedAuthStr);
      this.router.navigate(['/user-panel', { user: JSON.stringify({ id: userId, login: this.createUserCommand.login, lastSigninDateTime: new Date(), wasSuccessfulSignin: true }) }]);
    });
  }
}
