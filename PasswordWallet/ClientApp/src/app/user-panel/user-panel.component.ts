import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../sign-in/contracts/user';


@Component({
  selector: 'user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {

  user: User = {
    id: 0,
    login: '',
    lastSigninDateTime: new Date(),
    wasSuccessfulSignin: true,
    sessionKey: '',
    addressIP: ''
  }

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    var userStr = this.route.snapshot.paramMap.get('user');
    if (userStr !== null) {
      this.user = JSON.parse(userStr);
    }
  }

}
