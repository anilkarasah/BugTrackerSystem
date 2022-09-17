import { Component, OnInit } from '@angular/core';
import User from '../../models/user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  public users: User[] = [];

  constructor(private userService: UserService) {
    this.userService.getAllUsers().subscribe((value) => (this.users = value));
  }

  ngOnInit(): void {}
}
