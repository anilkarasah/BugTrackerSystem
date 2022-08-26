import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Project, ProjectData } from 'src/app/models/project.model';
import { ContributorData } from 'src/app/models/user.model';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-contributor',
  templateUrl: './add-contributor.component.html',
  styleUrls: ['./add-contributor.component.css'],
})
export class AddContributorComponent implements OnInit {
  usersList!: ContributorData[];

  contributor!: ContributorData;
  project!: ProjectData;

  isProjectLoaded: boolean = false;
  isUsersLoaded: boolean = false;

  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe((params: ParamMap) => {
      const projectId = params.get('projectId')!;

      this.projectService.getProjectById(projectId).subscribe((value) => {
        this.project = { id: value.id, name: value.name };
        this.isProjectLoaded = true;
      });

      this.userService.getMinimalUsersData().subscribe((value) => {
        this.usersList = value;
        this.isUsersLoaded = true;
      });
    });
  }

  ngOnInit(): void {}

  addContributor() {
    this.projectService
      .addContributor(this.contributor.id, this.project.id)
      .subscribe(
        (value) => alert('User has successfully added as a contributor.'),
        (err) => alert(err.error.title ?? 'Something wrong happened')
      );
  }
}
