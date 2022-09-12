import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ProjectData } from 'src/app/models/project.model';
import { BugService } from 'src/app/services/bug.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-bug-report',
  templateUrl: './bug-report.component.html',
  styleUrls: ['./bug-report.component.css'],
})
export class BugReportComponent implements OnInit {
  projectsList!: ProjectData[];

  title!: string;
  description!: string;
  projectId!: string | null;

  isProjectInitial: boolean = false;
  initializedProject?: ProjectData;

  constructor(
    private bugService: BugService,
    private projectService: ProjectService,
    private router: Router
  ) {
    this.isProjectInitial = !!history.state.id;

    if (this.isProjectInitial) {
      const statedProject = history.state;
      this.initializedProject = {
        id: statedProject.id,
        name: statedProject.name,
      };
      this.projectId = this.initializedProject.id;
    } else {
      this.projectService
        .getMinimalProjectData()
        .subscribe((list) => (this.projectsList = list));
    }
  }

  ngOnInit(): void {}

  onSubmit() {
    this.bugService
      .reportBug({
        title: this.title,
        description: this.description,
        projectId: this.projectId,
      })
      .subscribe((value) => this.router.navigate(['/']));
  }
}
