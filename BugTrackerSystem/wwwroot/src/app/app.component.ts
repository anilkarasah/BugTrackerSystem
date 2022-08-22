import { Component } from '@angular/core';
import User from './models/user.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'BugTrackerUI';
  users: User[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.title = '';
  }

  login() {
    console.log('sa');
    return this.http
      .post<any>(
        `${environment.API_URL}/auth/login`,
        {
          email: 'merhaba@anilkarasah.com',
          password: 'pass1234',
        },
        { withCredentials: true }
      )
      .subscribe((data) => (this.title = data.name));
  }

  showBugs() {
    return this.http
      .get<User[]>(`${environment.API_URL}/users`, { withCredentials: true })
      .subscribe((data) => {
        this.users = data;
        console.log(this.users);
      });
  }
}
