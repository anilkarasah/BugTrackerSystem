import { Component, OnInit } from '@angular/core';
import Bug from '../../../models/bug.model';
import { BugService } from '../../../services/bug.service';

@Component({
  selector: 'app-bug-component',
  templateUrl: './bug-component.component.html',
  styleUrls: ['./bug-component.component.css'],
})
export class BugComponentComponent implements OnInit {
  public bugs: Bug[] = [];
  public title: string = 'Bug Reports';

  constructor(private bugService: BugService) {
    this.bugService.getAllBugs().subscribe((bugs) => (this.bugs = bugs));
  }

  ngOnInit(): void {}
}
