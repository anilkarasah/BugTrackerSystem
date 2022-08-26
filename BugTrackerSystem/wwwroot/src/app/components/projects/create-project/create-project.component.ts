import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css'],
})
export class CreateProjectComponent implements OnInit {
  name!: string;

  constructor(private projectService: ProjectService, private router: Router) {}

  ngOnInit(): void {}

  createProject() {
    this.projectService.createProject(this.name).subscribe((value) => {
      alert(`'${this.name}' has successfully created.`);
      this.router.navigate(['/projects']);
    });
  }
}
