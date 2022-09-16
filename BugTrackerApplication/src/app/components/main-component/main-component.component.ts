import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-component',
  templateUrl: './main-component.component.html',
  styleUrls: ['./main-component.component.css'],
})
export class MainComponent implements OnInit {
  title: string = 'Selamlar!';

  constructor() {}

  ngOnInit(): void {}

  log() {
    console.log(this.title);
  }
}