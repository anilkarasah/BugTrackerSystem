import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsComponent } from './projects.component';
import { ProjectsListModule } from './projects-list/projects-list.module';
import { ProjectService } from 'src/app/services/project.service';
import { ProjectArticleModule } from './project-article/project-article.module';
import { CreateProjectModule } from './create-project/create-project.module';
import { ProjectEditPageModule } from './project-edit-page/project-edit-page.module';
import { ProjectPageModule } from './project-page/project-page.module';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProjectRouteModule } from './project-route.module';

@NgModule({
  declarations: [ProjectsComponent],
  imports: [
    ProjectRouteModule,
    CommonModule,
    RouterModule,
    FormsModule,
    CreateProjectModule,
    ProjectsListModule,
    ProjectArticleModule,
    ProjectEditPageModule,
    ProjectPageModule,
  ],
  providers: [ProjectService],
  exports: [ProjectsComponent],
})
export class ProjectsModule {}
