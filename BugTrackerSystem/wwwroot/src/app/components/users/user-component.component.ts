import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import User from '../../models/user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-component',
  templateUrl: './user-component.component.html',
  styleUrls: ['./user-component.component.css'],
})
export class UserComponent implements OnInit {
  public users: User[] = [];
  public title: string = 'User List';

  constructor(private userService: UserService) {
    this.userService.getAllUsers().subscribe(
      (value) => (this.users = value),
      (err) => (this.title = err.message)
    );
  }

  ngOnInit(): void {}
}
