import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Bug } from 'src/app/models/bug.model';
import { BugService } from 'src/app/services/bug.service';

@Component({
  selector: 'app-bug-page',
  templateUrl: './bug-page.component.html',
  styleUrls: ['./bug-page.component.css'],
})
export class BugPageComponent implements OnInit {
  bugId!: Number;
  bug!: Bug;

  constructor(private bugService: BugService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      (params: ParamMap) => (this.bugId = +params.get('id')!)
    );
    this.bugService
      .getBugById(this.bugId)
      .subscribe((value) => (this.bug = value));
  }
}
