import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// COMPONENT IMPORTS
import { AuthorizationGuard } from './guards/authorization.guard';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { AdminComponent } from './components/admin/admin.component';
import { WelcomeComponent } from './components/welcome/welcome.component';

const appRoutes = [
  { path: '', component: WelcomeComponent },
  {
    path: 'bugs',
    loadChildren: () =>
      import('../app/components/bugs/bugs.module').then((m) => m.BugsModule),
  },
  {
    path: 'projects',
    loadChildren: () =>
      import('../app/components/projects/projects.module').then(
        (m) => m.ProjectsModule
      ),
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
  },
  {
    path: 'profile',
    loadChildren: () =>
      import('../app/components/profile/profile.module').then(
        (m) => m.ProfileModule
      ),
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  { path: '**', redirectTo: '/' },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(appRoutes)],
})
export class AppRouteModule {}
