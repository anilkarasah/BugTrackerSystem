import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Project } from 'src/app/models/project.model';
import { ProjectService } from 'src/app/services/project.service';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ContributorData } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

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
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!history.state.id) this.router.navigate(['/projects']);
    this.project = history.state;
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
          alert('User has successfully added as a contributor.');
          this.project.contributorNamesList.push(this.newContributor);
        },
        error: (err) => {
          let errMsg;
          if (err.error.title)
            errMsg = err.error.title.startsWith(
              "The instance of entity type 'ProjectUser'"
            )
              ? 'This user is already a contributor'
              : '';

          alert(errMsg ?? 'Something wrong happened');
        },
      });
  }

  deleteProject() {
    if (!confirm(`'${this.project.name}' will get deleted. Do you agree?`))
      return;

    this.projectService.deleteProject(this.project.id).subscribe({
      complete: () => {
        alert(`'${this.project.name}' has deleted.`);
        this.router.navigate(['/projects']);
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
          alert(
            `'${contributor.name}' is successfuly removed from this project.`
          );
          this.project.contributorNamesList =
            this.project.contributorNamesList.filter(
              (el) => el.id !== contributor.id
            );
        },
        error: (err) =>
          alert('Could not remove the contributor. Please try again later.'),
      });
  }

  isAuthorized(roles: string[]): boolean {
    return this.authService.isAuthorized(roles);
  }
}
