import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { AppRoutingModule } from './app-routing.module';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { PasswordWalletComponent } from './passwords/password-wallet/password-wallet.component';
import { PasswordDetailsComponent } from './passwords/password-details/password-details.component';
import { UserPanelComponent } from './user-panel/user-panel.component';
import { SharedPasswordWalletComponent } from './passwords/shared-password-wallet/shared-password-wallet.component';

@NgModule({
  declarations: [
    AppComponent,
    SignInComponent,
    SignUpComponent,
    UserPanelComponent,
    ChangePasswordComponent,
    PasswordWalletComponent,
    PasswordDetailsComponent,
    SharedPasswordWalletComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
