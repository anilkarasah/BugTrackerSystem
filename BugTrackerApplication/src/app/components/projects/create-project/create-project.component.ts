import { Component, OnInit } from '@angular/core';
import { NotifyService } from 'src/app/services/notify.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css'],
})
export class CreateProjectComponent implements OnInit {
  name!: string;

  constructor(
    private projectService: ProjectService,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {}

  createProject() {
    this.projectService.createProject(this.name).subscribe({
      complete: () => {
        this.notifyService.alertSuccess(
          `'${this.name}' has successfully created.`,
          2500,
          '/projects'
        );
      },
    });
  }
}
