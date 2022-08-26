import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-edit-page',
  templateUrl: './project-edit-page.component.html',
  styleUrls: ['./project-edit-page.component.css'],
})
export class ProjectEditPageComponent implements OnInit {
  name!: string;
  projectId!: string;

  constructor(
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.paramMap.subscribe(
      (params: ParamMap) => (this.projectId = params.get('projectId')!)
    );
  }

  ngOnInit(): void {}

  upsertProject() {
    this.projectService
      .upsertProject(this.projectId, this.name)
      .subscribe((value) => {
        alert('Project successfully modified.');
        this.router.navigate(['/projects', this.projectId]);
      });
  }
}
