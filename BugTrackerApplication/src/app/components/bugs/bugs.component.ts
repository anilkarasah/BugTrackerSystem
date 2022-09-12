import { Component, OnInit, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Bug } from '../../models/bug.model';
import { BugService } from '../../services/bug.service';

@Component({
  selector: 'app-bugs-component',
  templateUrl: './bugs.component.html',
  styleUrls: ['./bugs.component.css'],
})
export class BugsComponent implements OnInit {
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
