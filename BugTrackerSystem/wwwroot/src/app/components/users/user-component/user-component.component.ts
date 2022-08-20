import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import User from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-component',
  templateUrl: './user-component.component.html',
  styleUrls: ['./user-component.component.css'],
})
export class UserComponentComponent implements OnInit {
  public users: User[] = [];
  public title: string = 'User List';
  private accessToken: string =
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlMmM0NzUyMC01M2E4LTQ1YTQtYjJiNy1iNDZjYTliODI2Y2UiLCJqdGkiOiJjYWMwMDUwOC0zN2I1LTQyNzktYTAyZS0yMmE2ODJlODliZDgiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImV4cCI6MTY2MTcxMDk5MSwiaXNzIjoiQnVnIFRyYWNrZXIgU3lzdGVtIGJ5IEFuxLFsIEthcmHFn2FoIiwiYXVkIjoiUG9zdG1hbkNsaWVudCJ9.VkmWMDJz779y4KYon8azqBToiOYeI3NJyfUv3vmgh3M';

  constructor(http: HttpClient) {
    http
      .get<any>('/', {
        headers: { Authorization: `Bearer ${this.accessToken}` },
      })
      .subscribe(
        (res) => {
          this.users = res.$values;
          console.log(this.users);
        },
        (err) => console.error(err)
      );
  }

  ngOnInit(): void {}
}
