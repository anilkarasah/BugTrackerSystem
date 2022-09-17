import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Project } from 'src/app/models/project.model';
import { NotifyService } from 'src/app/services/notify.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-edit-page',
  templateUrl: './project-edit-page.component.html',
  styleUrls: ['./project-edit-page.component.css'],
})
export class ProjectEditPageComponent implements OnInit {
  name!: string;

  project!: Project;

  constructor(
    private projectService: ProjectService,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {
    if (!history.state.id) window.location.replace('/projects');
    this.project = history.state;
  }

  upsertProject() {
    this.projectService.upsertProject(this.project.id, this.name).subscribe({
      complete: () => {
        this.notifyService.alertSuccess(
          'Project successfully modified.',
          2500,
          `/projects/${this.project.id}`
        );
      },
    });
  }
}
