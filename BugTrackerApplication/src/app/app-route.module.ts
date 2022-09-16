import { Component, CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// COMPONENT IMPORTS
import { BugsComponent } from './components/bugs/bugs.component';
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
import { ProjectsListComponent } from './components/projects/projects-list/projects-list.component';
import { ProjectsModule } from './components/projects/projects.module';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { BugListComponent } from './components/bugs/bug-list/bug-list.component';

const appRoutes = [
  { path: '', component: WelcomeComponent },
  {
    path: 'bugs',
    component: BugsComponent,
    children: [
      {
        path: '',
        component: BugListComponent,
      },
      {
        path: 'report',
        component: BugReportComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: 'admin' },
      },
      {
        path: ':bugId',
        component: BugPageComponent,
      },
      {
        path: ':bugId/edit',
        component: BugEditPageComponent,
        canActivate: [AuthorizationGuard],
        data: { roles: 'admin' },
      },
    ],
  },
  {
    path: 'projects',
    component: ProjectsComponent,
    children: [
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
    ],
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
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
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(appRoutes)],
})
export class AppRouteModule {}
