import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { apiErrorIntercpetor } from './core/interceptors/api-error-response.interceptor';

import {
  MsalService,
  MsalGuard,
  MsalBroadcastService,
  MSAL_INSTANCE,
} from '@azure/msal-angular';
import { msalInstance } from './core/config/auth-config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withInterceptors([apiErrorIntercpetor]), withFetch()),
    provideAnimationsAsync(),
    { provide: MSAL_INSTANCE, useValue: msalInstance },
    MsalService,
    MsalGuard,
    MsalBroadcastService,
  ],
};
