import { inject, Injectable, signal } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { loginRequestData } from '../config/auth-config';
import { AuthenticationResult } from '@azure/msal-browser';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private msal = inject(MsalService);
  signedInAccount = signal(this.msal.instance.getActiveAccount());

  constructor() {
    this.msal.instance
      .handleRedirectPromise()
      .then((res) => {
        console.log(res);
      })
      .catch((err) => {
        console.error(err);
      });
  }

  azureLogin() {
    this.msal
      .loginPopup(loginRequestData)
      .subscribe((response: AuthenticationResult) => {
        this.signedInAccount.set(response.account);
        this.msal.instance.setActiveAccount(response.account);
      });
  }

  azureLogout() {
    this.msal.logout();
    this.signedInAccount.set(null);
    this.msal.instance.setActiveAccount(null);
  }
}
