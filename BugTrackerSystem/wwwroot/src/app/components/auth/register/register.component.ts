import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  name!: string;
  email!: string;
  password!: string;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  register() {
    this.authService
      .register(this.name, this.email, this.password)
      .subscribe((value) => window.location.replace('/login'));
  }
}
