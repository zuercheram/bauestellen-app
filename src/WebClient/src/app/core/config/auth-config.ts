import {
  PublicClientApplication,
  Configuration,
  BrowserCacheLocation,
} from '@azure/msal-browser';

export const msalConfig: Configuration = {
  auth: {
    clientId: '37694761-f8d0-46fd-8cc6-1b91f6c1f0e0',
    authority:
      'https://login.microsoftonline.com/6f488316-257e-423e-99aa-d66b4f8b3c28',
    redirectUri: 'https://localhost:7276',
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage,
    storeAuthStateInCookie: true,
  },
};

export const loginRequestData = {
  scopes: [
    'User.Read',
    'api://91f3ad96-a695-42ef-86ca-1993b6de9812/Users.Read',
    'api://3b9b5f54-7acb-401c-84a1-71c6abbea963/Project.Read',
    'api://3b9b5f54-7acb-401c-84a1-71c6abbea963/Projects.Write',
  ],
};

export const loginRequestUserData = {
  scopes: ['api://91f3ad96-a695-42ef-86ca-1993b6de9812/Users.Read'],
};

export const loginRequestProjectData = {
  scopes: [
    'api://3b9b5f54-7acb-401c-84a1-71c6abbea963/Project.Read',
    'api://3b9b5f54-7acb-401c-84a1-71c6abbea963/Projects.Write',
  ],
};

export const msalInstance = new PublicClientApplication(msalConfig);
