import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import User, { ContributorData } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.API_URL}/users`);
  }

  getMinimalUsersData(): Observable<ContributorData[]> {
    return this.http.get<ContributorData[]>(
      `${environment.API_URL}/users/mini`
    );
  }

  getProfile(): Observable<User> {
    return this.http.get<User>(`${environment.API_URL}/me`);
  }
}
