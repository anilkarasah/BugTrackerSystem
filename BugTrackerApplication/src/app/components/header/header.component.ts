import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  @Input() title: string = '';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  isAuthorized(role: string): boolean {
    return this.authService.isAuthorized(role);
  }
}
