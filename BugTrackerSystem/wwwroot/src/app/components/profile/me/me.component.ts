import { Component, OnInit } from '@angular/core';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ProjectData } from 'src/app/models/project.model';
import User from 'src/app/models/user.model';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-me',
  templateUrl: './me.component.html',
  styleUrls: ['./me.component.css'],
})
export class MeComponent implements OnInit {
  public user!: User;
  public isLoaded: boolean = false;
  faXmark = faXmark;

  constructor(
    private userService: UserService,
    private projectService: ProjectService
  ) {
    this.userService.getProfile().subscribe((value) => {
      this.user = value;
      this.isLoaded = true;
    });
  }

  ngOnInit(): void {}

  removeContributor(contribution: ProjectData) {
    if (
      !confirm(
        `Do you really want remove your contribution from '${contribution.name}'?`
      )
    )
      return;

    this.projectService
      .removeContributor(this.user.id, contribution.id)
      .subscribe((value) => {
        alert(
          `You are no longer a contributor of the project '${contribution.name}'`
        );

        window.location.reload();
      });
  }
}
