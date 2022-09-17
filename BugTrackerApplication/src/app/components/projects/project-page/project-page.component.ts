import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Project } from 'src/app/models/project.model';
import { ProjectService } from 'src/app/services/project.service';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ContributorData } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { NotifyService } from 'src/app/services/notify.service';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css'],
})
export class ProjectPageComponent implements OnInit {
  project!: Project;

  faXmark = faXmark;

  isContributorFormShown: boolean = false;
  usersList: ContributorData[] = [];
  newContributor!: ContributorData;

  constructor(
    private projectService: ProjectService,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {
    // Is the project provided through state?
    if (history.state.id) {
      this.project = history.state;
      return;
    }

    // Is the project ID given through query parameters?
    let projectId;
    this.route.paramMap.subscribe((params: ParamMap) => {
      projectId = params.get('projectId');
    });
    if (projectId) {
      this.projectService
        .getProjectById(projectId)
        .subscribe((value) => (this.project = value));
      return;
    }

    // If not both, then redirect to lists page
    this.router.navigate(['/projects']);
  }

  openNewContributorForm() {
    this.userService.getMinimalUsersData().subscribe((users) => {
      this.usersList = users.filter(
        (list) =>
          !this.project.contributorNamesList.some(
            (filter) => filter.id === list.id
          )
      );

      this.isContributorFormShown = true;
    });
  }

  submitNewContributor() {
    this.projectService
      .addContributor(this.newContributor.id, this.project.id)
      .subscribe({
        complete: () => {
          this.project.contributorNamesList.push(this.newContributor);
          this.notifyService.alertSuccess(
            'User has successfully added as a contributor.'
          );
        },
        error: (err) => {
          let errMsg = 'Something wrong happened';
          if (
            err.error.title &&
            err.error.title.startsWith(
              "The instance of entity type 'ProjectUser'"
            )
          )
            errMsg = 'This user is already a contributor';

          this.notifyService.alertError(errMsg);
        },
      });
  }

  deleteProject() {
    if (!confirm(`'${this.project.name}' will get deleted. Do you agree?`))
      return;

    this.projectService.deleteProject(this.project.id).subscribe({
      complete: () => {
        this.notifyService.alertSuccess(
          `'${this.project.name}' has deleted.`,
          2500,
          '/projects'
        );
      },
    });
  }

  removeContributor(contributor: ContributorData) {
    if (
      !confirm(
        `Contributor '${contributor.name}' will be removed from the project '${this.project.name}'. Do you agree?`
      )
    )
      return;

    this.projectService
      .removeContributor(contributor.id, this.project.id)
      .subscribe({
        complete: () => {
          this.notifyService.alertSuccess(
            `'${contributor.name}' has successfully removed from this project.`
          );

          this.project.contributorNamesList =
            this.project.contributorNamesList.filter(
              (el) => el.id !== contributor.id
            );
        },
        error: (err) => {
          this.notifyService.alertError(
            'Could not remove the contributor. Please try again later.'
          );
        },
      });
  }

  isAuthorized(role: string): boolean {
    return this.authService.isAuthorized(role);
  }
}
