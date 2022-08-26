import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Project } from 'src/app/models/project.model';
import { ProjectService } from 'src/app/services/project.service';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ContributorData } from 'src/app/models/user.model';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css'],
})
export class ProjectPageComponent implements OnInit {
  projectId!: string;
  project!: Project;
  faXmark = faXmark;

  constructor(
    private projectService: ProjectService,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe(
      (params: ParamMap) => (this.projectId = params.get('id')!)
    );
    this.projectService
      .getProjectById(this.projectId)
      .subscribe((value) => (this.project = value));
  }

  ngOnInit(): void {}

  removeContributor(contributor: ContributorData) {
    if (
      !confirm(
        `Contributor '${contributor.name}' will be removed from the project '${this.project.name}'. Do you agree?`
      )
    )
      return;

    this.projectService
      .removeContributor(contributor.id, this.projectId)
      .subscribe(
        (value) => {
          alert(
            `'${contributor.name}' is successfuly removed from this project.`
          );
          window.location.reload();
        },
        (err) =>
          alert('Could not remove the contributor. Please try again later.')
      );
  }
}
