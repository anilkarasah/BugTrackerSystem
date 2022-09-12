import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { DecodedUser } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent implements OnInit {
  authenticatedUser?: DecodedUser;
  authenticatedFlag!: boolean;

  isOnLoginPage: boolean = true;

  constructor(private authService: AuthService, private router: Router) {
    this.authenticatedUser = this.authService.decodeStoredJwt();
    this.authService
      .isAuthenticated()
      .subscribe((value) => (this.authenticatedFlag = value));
  }

  ngOnInit(): void {}

  toggleAuthPage() {
    this.isOnLoginPage = !this.isOnLoginPage;
  }

  logout() {
    this.authService.logout().subscribe({
      complete: () => {
        this.authenticatedUser = undefined;
        this.authService.setAuthenticated(false);
        this.router.navigate(['/']);
      },
    });
  }
}
