import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import User, { UpsertUser } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent implements OnInit {
  public user!: User;
  public isLoaded: boolean = false;

  // request!: UpsertUser;
  name!: string;
  email!: string;
  currentPassword!: string;
  newPassword!: string;

  constructor(private userService: UserService, private router: Router) {
    this.userService.getProfile().subscribe((value) => {
      this.user = value;
      this.isLoaded = true;
    });
  }

  ngOnInit(): void {}

  updateProfile() {
    this.userService
      .updateMe({
        name: this.name,
        email: this.email,
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
      })
      .subscribe((value) => {
        alert('Successfully updated your profile.');
        this.router.navigate(['/profile']);
      });
  }
}
