import { Component, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project.model';
import { AuthService } from 'src/app/services/auth.service';
import { ProjectService } from 'src/app/services/project.service';
import { ProjectArticleComponent } from '../project-article/project-article.component';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
})
export class ProjectsListComponent implements OnInit {
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

  isAuthorized(role: string): boolean {
    return this.authService.isAuthorized(role);
  }
}
