import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsListComponent } from './projects-list/projects-list.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { AuthorizationGuard } from 'src/app/guards/authorization.guard';
import { ProjectPageComponent } from './project-page/project-page.component';
import { ProjectEditPageComponent } from './project-edit-page/project-edit-page.component';
import { RouterModule } from '@angular/router';

const childrenRoutes = [
  {
    path: '',
    component: ProjectsListComponent,
  },
  {
    path: 'create',
    component: CreateProjectComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
  },
  {
    path: ':projectId',
    component: ProjectPageComponent,
  },
  {
    path: ':projectId/edit',
    component: ProjectEditPageComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(childrenRoutes)],
  exports: [RouterModule],
})
export class ProjectRouteModule {}
