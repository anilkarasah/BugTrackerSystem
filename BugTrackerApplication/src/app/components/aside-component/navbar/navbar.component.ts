import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
})
export class NavbarComponent implements OnInit {
  @Input() page?: string;
  isAuthenticated: boolean = false;

  constructor(private authService: AuthService) {
    this.authService
      .isAuthenticated()
      .subscribe((value) => (this.isAuthenticated = value));
  }

  ngOnInit(): void {}

  isAuthorized(roles: string): boolean {
    return this.authService.isAuthorized(roles);
  }
}
