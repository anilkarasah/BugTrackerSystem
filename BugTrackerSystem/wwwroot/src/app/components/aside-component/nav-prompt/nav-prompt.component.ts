import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-prompt',
  templateUrl: './nav-prompt.component.html',
})
export class NavPromptComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  isUserAuthenticated(): boolean {
    return false;
  }

  logout() {
    this.authService.logout().subscribe((value) => console.log(value));
  }
}
