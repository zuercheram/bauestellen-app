import {
  HttpEvent,
  HttpEventType,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { inject } from '@angular/core';

export function apiErrorIntercpetor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  return next(req).pipe(
    tap((event) => {
      if (event.type === HttpEventType.Response) {
        const _snackBar = inject(MatSnackBar);

        if (event.status !== 200) {
          _snackBar.open('An error occurred', 'Close', {
            horizontalPosition: 'end' as MatSnackBarHorizontalPosition,
            verticalPosition: 'top' as MatSnackBarVerticalPosition,
            duration: 7000,
          });
        }
        console.log(req.url, 'returned a response with status', event.status);
      }
    })
  );
}
