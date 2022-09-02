import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import User, { ContributorData, UpsertUser } from '../models/user.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.API_URL}/users`);
  }

  getMinimalUsersData(): Observable<ContributorData[]> {
    return this.http.get<ContributorData[]>(
      `${environment.API_URL}/users/mini`
    );
  }

  getProfile(userId: string | null): Observable<User> {
    if (!userId) return this.http.get<User>(`${environment.API_URL}/me`);

    return this.http.get<User>(`${environment.API_URL}/users/${userId}`);
  }

  updateProfile(upsertRequest: UpsertUser): Observable<User> {
    if (this.authService.isAuthorized(['admin']))
      return this.http.patch<User>(
        `${environment.API_URL}/users/${upsertRequest.id}`,
        {
          name: upsertRequest.name,
          email: upsertRequest.email,
          newPassword: upsertRequest.newPassword,
          role: upsertRequest.role,
        }
      );

    return this.http.patch<User>(`${environment.API_URL}/me`, {
      name: upsertRequest.name,
      email: upsertRequest.email,
      currentPassword: upsertRequest.currentPassword,
      newPassword: upsertRequest.newPassword,
    });
  }
}
