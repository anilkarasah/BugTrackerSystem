import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
})
export class NavbarComponent implements OnInit {
  @Input() page?: string;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  isAuthenticated() {
    return this.authService.isAuthenticated();
  }

  isAuthorized(roles: string[]): boolean {
    return this.authService.isAuthorized(roles);
  }
}
