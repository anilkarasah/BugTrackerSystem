import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// COMPONENT IMPORTS
import { BugComponent } from './components/bugs/bug.component';
import { BugEditPageComponent } from './components/bugs/bug-edit-page/bug-edit-page.component';
import { AuthorizationGuard } from './guards/authorization.guard';
import { BugReportComponent } from './components/bugs/bug-report/bug-report.component';
import { BugPageComponent } from './components/bugs/bug-page/bug-page.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { CreateProjectComponent } from './components/projects/create-project/create-project.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ProjectPageComponent } from './components/projects/project-page/project-page.component';
import { ProjectEditPageComponent } from './components/projects/project-edit-page/project-edit-page.component';
import { AdminComponent } from './components/admin/admin.component';
import { ProfileComponent } from './components/profile/profile.component';

const appRoutes = [
  // { path: '', redirectTo: '/bugs', pathMatch: 'full' },
  {
    path: 'bugs',
    component: BugComponent,
    children: [
      {
        path: 'report',
        component: BugReportComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: ['admin'] },
      },
      {
        path: 'report/:projectId',
        component: BugReportComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: ['admin'] },
      },
      {
        path: 'edit/:id',
        component: BugEditPageComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: ['admin'] },
      },
      { path: ':id', component: BugPageComponent },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'projects',
    component: ProjectsComponent,
    children: [
      {
        path: 'create',
        component: CreateProjectComponent,
        canActivate: [AuthenticationGuard],
      },
      { path: ':id', component: ProjectPageComponent },
      {
        path: 'edit/:projectId',
        component: ProjectEditPageComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: ['admin'] },
      },
    ],
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: ['admin'] },
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthenticationGuard],
    children: [
      {
        path: ':userId',
        component: ProfileComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(appRoutes, { useHash: true })],
})
export class AppRouteModule {}
