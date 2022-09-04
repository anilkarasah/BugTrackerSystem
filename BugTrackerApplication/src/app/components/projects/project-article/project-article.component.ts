import { Component, Input, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project.model';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/services/auth.service';
import { ProjectService } from 'src/app/services/project.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-project-article',
  templateUrl: './project-article.component.html',
  styleUrls: ['./project-article.component.css'],
})
export class ProjectArticleComponent implements OnInit {
  @Input() project!: Project;
  showProjectDetails: boolean = false;
  faEye = faEye;
  faEyeSlash = faEyeSlash;

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  toggleProjectDetails() {
    this.showProjectDetails = !this.showProjectDetails;
  }
}
