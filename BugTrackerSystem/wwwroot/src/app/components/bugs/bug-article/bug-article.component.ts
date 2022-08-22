import { Component, OnInit, Input } from '@angular/core';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import Bug from '../../../models/bug.model';

@Component({
  selector: 'app-bug-article',
  templateUrl: './bug-article.component.html',
  styleUrls: ['./bug-article.component.css'],
})
export class BugArticleComponent implements OnInit {
  public isFocused: boolean = false;
  public faIcon = faEye;
  @Input() bug!: Bug;

  constructor() {}

  ngOnInit(): void {}

  toggleBug(bug: Bug) {
    this.isFocused = !this.isFocused;
    this.faIcon = this.isFocused ? faEyeSlash : faEye;

    const section = document.querySelector(`#${bug.id}`);
    section?.classList.remove(this.isFocused ? 'bug-hidden' : 'bug-shown');
    section?.classList.add(this.isFocused ? 'bug-shown' : 'bug-hidden');
  }
}
