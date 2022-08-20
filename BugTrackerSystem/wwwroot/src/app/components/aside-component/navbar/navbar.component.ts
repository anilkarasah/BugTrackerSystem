import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
})
export class NavbarComponent implements OnInit {
  @Input() page?: string;

  constructor() {}

  ngOnInit(): void {
    document.querySelector('a.nav--active')?.classList.remove('nav--active');
    document.getElementById(`nav-${this.page}`)?.classList.add('nav--active');
  }
}
