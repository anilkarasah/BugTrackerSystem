import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectArticleModule } from '../project-article/project-article.module';
import { ProjectsListComponent } from './projects-list.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [ProjectsListComponent],
  imports: [CommonModule, RouterModule, ProjectArticleModule],
  exports: [ProjectsListComponent],
})
export class ProjectsListModule {}
