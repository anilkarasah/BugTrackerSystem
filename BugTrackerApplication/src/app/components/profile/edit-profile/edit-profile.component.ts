import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import User from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent implements OnInit {
  @Input() user!: User;

  name!: string;
  email!: string;
  currentPassword!: string;
  newPassword!: string;
  role?: string;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {}

  updateProfile() {
    const role = this.role ? 'admin' : 'user';

    this.userService
      .updateProfile({
        id: this.user.id,
        name: this.name,
        email: this.email,
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
        role,
      })
      .subscribe({
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
