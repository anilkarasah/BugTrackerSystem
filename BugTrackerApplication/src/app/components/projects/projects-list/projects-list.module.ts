import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectArticleModule } from '../project-article/project-article.module';
import { ProjectsListComponent } from './projects-list.component';
import { RouterModule } from '@angular/router';
import { HeaderModule } from '../../header/header.module';

@NgModule({
  declarations: [ProjectsListComponent],
  imports: [CommonModule, RouterModule, ProjectArticleModule, HeaderModule],
  exports: [ProjectsListComponent],
})
export class ProjectsListModule {}
