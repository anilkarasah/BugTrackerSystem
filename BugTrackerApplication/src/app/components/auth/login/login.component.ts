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
    this.authService
      .login(this.email, this.password)
      .subscribe((loginResponse) => {
        // console.log(this.authService.getToken());
        localStorage.setItem('jwt', loginResponse.token);
        this.authService.setAuthenticatedUserFromLoginResponse(loginResponse);
        this.authService.setAuthenticated(true);
        this.notifyService.alertSuccess('Logged in successfully.');
      });
  }
}
