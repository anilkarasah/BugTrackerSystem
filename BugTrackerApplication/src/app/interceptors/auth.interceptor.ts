import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    var requestHeaders: {
      setHeaders: object;
      withCredentials: boolean;
      Authorization: string | null;
    } = {
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
      },
      withCredentials: true,
      Authorization: null,
    };

    const jwt = this.authService.getToken();
    if (!!jwt) {
      request.headers.set('Authorization', jwt);
    }

    request = request.clone({
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
      },
      withCredentials: true,
    });

    return next.handle(request);
  }
}
