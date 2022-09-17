import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err instanceof HttpErrorResponse) {
          if (err.error instanceof ErrorEvent) {
            console.log('Error event');
          } else {
            const errorMessage = err.error.title ?? err.message;
            this.toastr.error(errorMessage, `${err.status}`);
          }
        } else {
          console.log('An error occured');
        }

        return throwError(() => new Error(err.statusText));
      })
    );
  }
}
