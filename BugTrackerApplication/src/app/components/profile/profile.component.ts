import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ProjectData } from 'src/app/models/project.model';
import User, { DecodedUser } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotifyService } from 'src/app/services/notify.service';
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
    private route: ActivatedRoute,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {
    // console.log(history.state);

    if (history.state.id) this.updateFlags(history.state);
    else
      this.route.paramMap.subscribe((params: ParamMap) =>
        this.userService
          .getProfile(params.get('userId'))
          .subscribe((value: User) => this.updateFlags(value))
      );
  }

  private updateFlags(userData: User): void {
    this.user = userData;

    this.authService.getAuthenticatedUser().subscribe((decodedUser) => {
      this.canEditProfile =
        decodedUser?.id === userData.id ||
        this.authService.isAuthorized('admin');

      this.isLoaded = true;
    });
  }

  toggleEditPage() {
    this.isEditPageVisible = !this.isEditPageVisible;
  }

  isAuthorized(role: string): boolean {
    return this.authService.isAuthorized(role);
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
          this.notifyService.alertSuccess(
            `You are no longer a contributor of the project '${contribution.name}'`,
            2500,
            null
          );
        },
      });
  }
}
