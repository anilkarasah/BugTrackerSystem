import { Component, OnInit } from '@angular/core';
import { NotifyService } from 'src/app/services/notify.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  email!: string;
  password!: string;

  constructor(
    private authService: AuthService,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {}

  login() {
    this.authService.login(this.email, this.password).subscribe((value) => {
      this.authService.setAuthenticatedUserFromLoginResponse(value);
      this.authService.setAuthenticated(true);
      this.notifyService.alertSuccess('Logged in successfully.');
    });
  }
}
