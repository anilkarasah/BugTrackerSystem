import { Component, OnInit, Input } from '@angular/core';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/services/auth.service';
import { BugService } from 'src/app/services/bug.service';
import { Bug } from '../../../models/bug.model';

@Component({
  selector: 'app-bug-article',
  templateUrl: './bug-article.component.html',
  styleUrls: ['./bug-article.component.css'],
})
export class BugArticleComponent implements OnInit {
  @Input() bug!: Bug;
  showBugDetails: boolean = false;
  faEye = faEye;
  faEyeSlash = faEyeSlash;

  constructor(
    private bugService: BugService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {}

  toggleBugDetails() {
    this.showBugDetails = !this.showBugDetails;
  }

  deleteBug(): void {
    if (!confirm(`#BT-${this.bug.id} will be deleted. Do you confirm?`)) return;

    this.bugService.deleteBug(this.bug).subscribe((value) => {
      alert(`#BT-${this.bug.id} is deleted.`);
      window.location.reload();
    });
  }

  isAuthorized(roles: string[]): boolean {
    return this.authService.isAuthorized(roles);
  }
}
