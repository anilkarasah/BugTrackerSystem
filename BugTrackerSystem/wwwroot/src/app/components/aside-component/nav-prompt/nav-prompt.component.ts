import { Component, OnInit } from '@angular/core';
import { DecodedUser } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-prompt',
  templateUrl: './nav-prompt.component.html',
})
export class NavPromptComponent implements OnInit {
  authenticatedUser?: DecodedUser;
  isAuthenticated!: boolean;

  constructor(private authService: AuthService) {
    this.authenticatedUser = this.authService.decodeStoredJwt();
    this.isAuthenticated = !!this.authenticatedUser;
  }

  ngOnInit(): void {}

  logout() {
    this.authService
      .logout()
      .subscribe((value) => window.location.replace('/'));
  }
}
