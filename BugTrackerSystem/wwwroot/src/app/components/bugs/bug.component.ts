import { Component, OnInit, Output } from '@angular/core';
import { Bug } from '../../models/bug.model';
import { BugService } from '../../services/bug.service';

@Component({
  selector: 'app-bug-component',
  templateUrl: './bug.component.html',
  styleUrls: ['./bug.component.css'],
})
export class BugComponent implements OnInit {
  public bugs: Bug[] = [];
  @Output() bugArticle?: Bug;

  constructor(private bugService: BugService) {
    this.bugService.getAllBugs().subscribe((bugs) => (this.bugs = bugs));
  }

  ngOnInit(): void {}
}
