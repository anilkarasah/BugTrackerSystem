import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginResponse, AuthResponse, DecodedUser } from '../models/user.model';

const authUrl: string = `${environment.API_URL}/auth`;

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private cookies: CookieService,
    private router: Router
  ) {
    const jwt = this.cookies.get('jwt');
    if (jwt) {
      const decodedJwt = this.jwtHelper.decodeToken(jwt);
      const tokenExpiresAt = decodedJwt['exp'];
      const currentTimestamp = (new Date().getTime() / 1000).toFixed(0);

      if (tokenExpiresAt <= currentTimestamp) {
        this.cookies.delete('jwt');
        alert('Your token has expired. Please log in again.');
        this.router.navigate(['/login']);
      }
    }
  }

  getToken(): string {
    return this.cookies.get('jwt');
  }

  register(
    name: string,
    email: string,
    password: string
  ): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${authUrl}/register`, {
      name,
      email,
      password,
    });
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${authUrl}/login`, {
      email,
      password,
    });
  }

  logout(): Observable<any> {
    return this.http.post<any>(`${authUrl}/logout`, undefined);
  }

  decodeStoredJwt(): DecodedUser | undefined {
    const jwt = this.cookies.get('jwt');
    if (!jwt) return undefined;

    const decodedJwt = this.jwtHelper.decodeToken(jwt);

    const idLocatedAt = 'sub';
    const nameLocatedAt =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const RoleLocatedAt =
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    const decodedUser: DecodedUser = {
      id: decodedJwt[idLocatedAt],
      name: decodedJwt[nameLocatedAt],
      role: decodedJwt[RoleLocatedAt],
    };

    return decodedUser;
  }

  isAuthenticated(): boolean {
    return !!this.cookies.get('jwt');
  }

  isAuthorized(requiredRoles: string[]): boolean {
    const decodedUser = this.decodeStoredJwt();
    if (!decodedUser) return false;

    let isUserAuthorized = false;
    requiredRoles.forEach((role) => {
      isUserAuthorized = decodedUser.role.toLowerCase() === role.toLowerCase();
    });

    return isUserAuthorized;
  }
}
