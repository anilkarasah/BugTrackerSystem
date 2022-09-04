import { Component, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project.model';
import { AuthService } from 'src/app/services/auth.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];

  constructor(
    private projectService: ProjectService,
    private authService: AuthService
  ) {
    this.projectService
      .getAllProjects()
      .subscribe((projects) => (this.projects = projects));
  }

  ngOnInit(): void {}

  isAuthorized(roles: string[]): boolean {
    return this.authService.isAuthorized(roles);
  }
}
