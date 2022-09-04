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
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.projectId = params.get('projectId');

      this.isProjectInitial = !!this.projectId;

      if (this.isProjectInitial) {
        console.log(`Initial ${this.projectId}`);
        this.projectService
          .getProjectById(this.projectId!)
          .subscribe((project) => (this.initializedProject = project));
      } else {
        this.projectService
          .getMinimalProjectData()
          .subscribe((list) => (this.projectsList = list));
      }
    });
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
