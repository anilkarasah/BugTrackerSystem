import { Component, OnInit, Output } from '@angular/core';
import { Bug } from 'src/app/models/bug.model';
import { AuthService } from 'src/app/services/auth.service';
import { BugService } from 'src/app/services/bug.service';

@Component({
  selector: 'app-bug-list',
  templateUrl: './bug-list.component.html',
  styleUrls: ['./bug-list.component.css'],
})
export class BugListComponent implements OnInit {
  public bugs: Bug[] = [];
  @Output() bugArticle?: Bug;

  constructor(
    private bugService: BugService,
    private authService: AuthService
  ) {
    this.bugService.getAllBugs().subscribe((bugs) => (this.bugs = bugs));
  }

  ngOnInit(): void {}

  isAuthorized(roles: string): boolean {
    return this.authService.isAuthorized(roles);
  }
}
