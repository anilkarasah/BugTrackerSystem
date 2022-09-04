import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Project } from 'src/app/models/project.model';
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
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!history.state.id) this.router.navigate(['/projects']);
    this.project = history.state;
  }

  upsertProject() {
    this.projectService
      .upsertProject(this.project.id, this.name)
      .subscribe((value) => {
        alert('Project successfully modified.');
        this.router.navigate(['/projects', this.project.id]);
      });
  }
}
