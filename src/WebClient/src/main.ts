import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { msalInstance } from './app/core/config/auth-config';

msalInstance
  .initialize()
  .then(() => {
    console.log('MSAL instance initialized successfully!');
    bootstrapApplication(AppComponent, appConfig).catch((err) =>
      console.error(err)
    );
  })
  .catch((err) => console.error('MSAL instance failed to initialize', err));
