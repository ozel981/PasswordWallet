import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UpdateUserCommand } from '../commands/createUserCommand';
import { SignInData } from '../contracts/signInData';
import { UserService } from '../repositories/user.service';
import { AUTHSTR } from '../settings';

@Component({
  selector: 'change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  updateUserCommand: UpdateUserCommand = {
    login: '',
    password: '',
    isPasswordKeptAsHash: false
  }

  signInData: SignInData = {
    login: '',
    password: ''
  }

  constructor(private userService: UserService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    var login = this.route.snapshot.paramMap.get('login');
    if (login !== null) {
      this.updateUserCommand.login = login;
      this.signInData.login = login;
    }
  }

  changePassword() {
    this.userService.changePassword(this.signInData, this.updateUserCommand).subscribe(response => {
      localStorage.setItem(AUTHSTR, this.userService.getAuthStr({ login: this.signInData.login, password: this.updateUserCommand.password }));
      this.router.navigate(['/user-panel', { login: this.signInData.login }]);
    });
  }
}
