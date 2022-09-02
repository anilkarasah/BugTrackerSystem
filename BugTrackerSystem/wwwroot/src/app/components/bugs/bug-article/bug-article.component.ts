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

  constructor() {}

  ngOnInit(): void {}

  toggleBugDetails() {
    this.showBugDetails = !this.showBugDetails;
  }
}
