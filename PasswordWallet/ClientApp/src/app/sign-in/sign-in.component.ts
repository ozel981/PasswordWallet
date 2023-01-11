import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SignInData } from '../contracts/signInData';
import { UserService } from '../repositories/user.service';
import { AUTHSTR } from '../settings';

@Component({
  selector: 'sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  user: SignInData = {
    login: 'Wojtek',
    password: 'Qwerty123'
  };

  errorResponse: HttpErrorResponse | undefined = undefined;

  constructor(private userService: UserService, private router: Router) {
  }

  signin() {
    this.userService.signIn(this.user).subscribe({
      next: user => {
        this.errorResponse = undefined;
        localStorage.setItem(AUTHSTR, this.userService.getAuthStr(this.user));
        this.router.navigate(['/user-panel', { user: JSON.stringify(user) }]);
      },
      error: (error: HttpErrorResponse) => {
        this.errorResponse = error;
      },
    });
  }

  ngOnInit(): void { }

}
