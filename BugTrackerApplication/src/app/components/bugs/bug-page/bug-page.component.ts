import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Bug } from 'src/app/models/bug.model';
import { AuthService } from 'src/app/services/auth.service';
import { BugService } from 'src/app/services/bug.service';

@Component({
  selector: 'app-bug-page',
  templateUrl: './bug-page.component.html',
  styleUrls: ['./bug-page.component.css'],
})
export class BugPageComponent implements OnInit {
  bugId!: Number;
  bug!: Bug;

  constructor(
    private bugService: BugService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe(
      (params: ParamMap) => (this.bugId = +params.get('bugId')!)
    );
    this.bugService
      .getBugById(this.bugId)
      .subscribe((value) => (this.bug = value));
  }

  ngOnInit(): void {}

  deleteBug(): void {
    if (!confirm(`#BT-${this.bug.id} will be deleted. Do you confirm?`)) return;

    this.bugService.deleteBug(this.bug).subscribe((value) => {
      alert(`#BT-${this.bug.id} is deleted.`);
      window.location.replace('/');
    });
  }

  isAuthorized(roles: string): boolean {
    return this.authService.isAuthorized(roles);
  }
}
