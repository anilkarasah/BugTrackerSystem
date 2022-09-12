import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Bug } from 'src/app/models/bug.model';
import { BugService } from 'src/app/services/bug.service';

@Component({
  selector: 'app-bug-edit-page',
  templateUrl: './bug-edit-page.component.html',
  styleUrls: ['./bug-edit-page.component.css'],
})
export class BugEditPageComponent implements OnInit {
  bugId!: Number;
  bug!: Bug;
  isFullyRendered: boolean = false;

  title!: string;
  description!: string;
  status!: string;

  constructor(
    private bugService: BugService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.paramMap.subscribe(
      (params: ParamMap) => (this.bugId = +params.get('bugId')!)
    );
    this.bugService.getBugById(this.bugId).subscribe((value) => {
      this.bug = value;
      this.title = this.bug.title;
      this.description = this.bug.description;
      this.status = this.bug.status;
      this.isFullyRendered = true;
    });
  }

  ngOnInit(): void {}

  onSubmit() {
    this.bugService
      .upsertBug(
        {
          title: this.title,
          description: this.description,
          status: this.status,
        },
        this.bugId
      )
      .subscribe((response) => {
        this.bug = response;
        setTimeout(() => this.router.navigate(['/bugs', this.bugId]), 1000);
      });
  }
}
