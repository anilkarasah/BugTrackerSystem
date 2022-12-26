import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginResponse, AuthResponse, DecodedUser } from '../models/user.model';
import { NotifyService } from './notify.service';

const authUrl: string = `${environment.API_URL}/auth`;

const keyOfID = 'sub';
const keyOfName = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
const keyOfRole =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private userAuthenticationState: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);

  private authenticatedUser: BehaviorSubject<DecodedUser | undefined> =
    new BehaviorSubject<DecodedUser | undefined>(undefined);

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private cookies: CookieService,
    private notifyService: NotifyService
  ) {
    const jwt = this.getToken();
    if (!jwt) return;

    const decodedJwt = this.jwtHelper.decodeToken(jwt);
    const tokenExpiresAt = decodedJwt['exp'];
    const currentTimestamp = (new Date().getTime() / 1000).toFixed(0);

    if (tokenExpiresAt <= currentTimestamp) {
      this.cookies.delete('jwt');
      this.authenticatedUser.next(undefined);
      this.userAuthenticationState.next(false);
      this.notifyService.alertError('Please log in again.', 'Token Expired');
    } else {
      this.userAuthenticationState.next(true);
    }
  }

  getToken(): string {
    const jwt = this.cookies.get('jwt') ?? localStorage.getItem('jwt');
    return jwt;
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

  setAuthenticatedUser(user: DecodedUser | undefined): void {
    this.authenticatedUser.next(user);
  }

  setAuthenticatedUserFromLoginResponse(response: LoginResponse): void {
    this.authenticatedUser.next({
      id: response.user.id,
      name: response.user.name,
      role: response.user.role,
    });
  }

  getAuthenticatedUser(): Observable<DecodedUser | undefined> {
    const jwt = this.getToken();
    const decodedJwt = this.jwtHelper.decodeToken(jwt);
    if (!jwt || !decodedJwt) {
      this.authenticatedUser.next(undefined);
    } else {
      this.authenticatedUser.next({
        id: decodedJwt[keyOfID],
        name: decodedJwt[keyOfName],
        role: decodedJwt[keyOfRole],
      });
    }

    return this.authenticatedUser.asObservable();
  }

  getAuthenticated(): Observable<boolean> {
    return this.userAuthenticationState.asObservable();
  }
  setAuthenticated(value: boolean): void {
    this.userAuthenticationState.next(value);
  }

  isAuthenticated(): boolean {
    return this.userAuthenticationState.getValue();
  }

  isAuthorized(requiredRole: string): boolean {
    const decodedUser = this.authenticatedUser.value;
    if (!decodedUser) return false;

    return decodedUser.role.toLowerCase() === requiredRole.toLowerCase();
  }
}
