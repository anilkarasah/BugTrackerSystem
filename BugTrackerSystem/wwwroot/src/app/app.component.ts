import { Component } from '@angular/core';
import User from './models/user.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'BugTrackerUI';
  users: User[] = [];

  constructor(
    private authService: AuthService,
    private jwtHelper: JwtHelperService
  ) {}

  ngOnInit(): void {}
}
