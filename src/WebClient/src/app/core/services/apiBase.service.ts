import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { loginRequestData } from '../config/auth-config';
import { AuthenticationResult } from '@azure/msal-browser';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiBaseService {
  private msal = inject(MsalService);
  private httpClient = inject(HttpClient);

  public async get<T>(url: string): Promise<Observable<T>> {
    const httpRes: AuthenticationResult =
      await this.msal.instance.acquireTokenSilent(loginRequestData);
    return this.httpClient.get<T>(url, {
      headers: {
        Authorization: `Bearer ${httpRes.accessToken}`,
      },
    });
  }
}
