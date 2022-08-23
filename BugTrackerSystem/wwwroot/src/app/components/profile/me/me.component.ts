import { Component, OnInit } from '@angular/core';
import User from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-me',
  templateUrl: './me.component.html',
  styleUrls: ['./me.component.css'],
})
export class MeComponent implements OnInit {
  public user!: User;
  public isLoaded: boolean = false;

  constructor(private userService: UserService) {
    this.userService.getProfile().subscribe((value) => {
      this.user = value;
      this.isLoaded = true;
    });
  }

  ngOnInit(): void {}
}
