import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  email!: string;
  password!: string;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  login() {
    this.authService.login(this.email, this.password).subscribe({
      complete: () => this.router.navigate(['/']),
      error: (err) => {
        if (err.error && err.error.title) alert(err.error.title);
        else alert('Something wrong happened. Please try again later.');
      },
    });
  }
}
