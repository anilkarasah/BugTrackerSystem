import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ProjectData } from 'src/app/models/project.model';
import User from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  public user!: User;
  public isLoaded: boolean = false;
  public isEditPageVisible: boolean = false;
  public canEditProfile: boolean = false;

  faXmark = faXmark;

  constructor(
    private userService: UserService,
    private projectService: ProjectService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (history.state.id) this.updateFlags(history.state);
    else
      this.route.paramMap.subscribe((params: ParamMap) =>
        this.userService
          .getProfile(params.get('userId'))
          .subscribe((value) => this.updateFlags(value))
      );
  }

  private updateFlags(userData: User): void {
    this.user = userData;
    if (
      this.authService.decodeStoredJwt()?.id === userData.id ||
      this.authService.isAuthorized('admin')
    )
      this.canEditProfile = true;

    this.isLoaded = true;
  }

  toggleEditPage() {
    this.isEditPageVisible = !this.isEditPageVisible;
  }

  isAuthorized(roles: string): boolean {
    return this.authService.isAuthorized(roles);
  }

  removeContributor(contribution: ProjectData) {
    if (
      !confirm(
        `Do you really want to remove your contribution from '${contribution.name}'?`
      )
    )
      return;

    this.projectService
      .removeContributor(this.user.id, contribution.id)
      .subscribe({
        complete: () => {
          alert(
            `You are no longer a contributor of the project '${contribution.name}'`
          );

          window.location.reload();
        },
      });
  }
}
