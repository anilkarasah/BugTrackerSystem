import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import User, { UpsertUser } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
})
export class EditProfileComponent implements OnInit {
  @Input() user!: User;

  name!: string;
  email!: string;
  currentPassword!: string;
  newPassword!: string;
  role?: boolean;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.role = this.user.role === 'admin';
  }

  updateProfile() {
    const role = this.role ? 'admin' : 'user';

    const newUser: UpsertUser = {
      id: this.user.id,
      name: this.name,
      email: this.email,
      currentPassword: this.currentPassword,
      newPassword: this.newPassword,
      role,
    };

    this.userService.updateProfile(newUser).subscribe({
      complete: () => {
        alert('Successfully updated the profile.');
        this.router.navigate(['/profile']);
      },
      error: (err) => {
        alert(err.error.title ?? 'Something went wrong.');
      },
    });
  }

  isAuthorized(roles: string) {
    return this.authService.isAuthorized(roles);
  }
}
