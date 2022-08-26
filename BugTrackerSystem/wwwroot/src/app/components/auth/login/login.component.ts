import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  email!: string;
  password!: string;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  login() {
    this.authService.login(this.email, this.password).subscribe(
      (value) => window.location.replace('/'),
      (err) => {
        if (err.error.title) alert(err.error.title);
        else alert('Something wrong happened. Please try again later.');
      }
    );
  }
}
