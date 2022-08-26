import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BugService } from 'src/app/services/bug.service';

@Component({
  selector: 'app-bug-report',
  templateUrl: './bug-report.component.html',
  styleUrls: ['./bug-report.component.css'],
})
export class BugReportComponent implements OnInit {
  projectsList!: any[];

  title!: string;
  description!: string;
  projectId!: string;

  constructor(private bugService: BugService, private router: Router) {
    this.bugService
      .getListOfProjects()
      .subscribe((list) => (this.projectsList = list));
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
