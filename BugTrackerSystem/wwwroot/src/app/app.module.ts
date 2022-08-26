import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';

import { AppComponent } from './app.component';
import { AsideComponent } from './components/aside-component/aside-component.component';
import { MainComponent } from './components/main-component/main-component.component';
import { BugComponent } from './components/bugs/bug.component';
import { UserComponent } from './components/users/user-component.component';
import { NavbarComponent } from './components/aside-component/navbar/navbar.component';
import { NavPromptComponent } from './components/aside-component/nav-prompt/nav-prompt.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { MeComponent } from './components/profile/me/me.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BugArticleComponent } from './components/bugs/bug-article/bug-article.component';
import { BugPageComponent } from './components/bugs/bug-page/bug-page.component';
import { BugEditPageComponent } from './components/bugs/bug-edit-page/bug-edit-page.component';
import { AuthInterceptor } from './guards/AuthInterceptor';
import { BugReportComponent } from './components/bugs/bug-report/bug-report.component';
import { AuthorizationGuard } from './guards/authorization.guard';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ProjectsComponent } from './components/projects/projects.component';
import { ProjectArticleComponent } from './components/projects/project-article/project-article.component';
import { ProjectPageComponent } from './components/projects/project-page/project-page.component';
import { ProjectEditPageComponent } from './components/projects/project-edit-page/project-edit-page.component';
import { AddContributorComponent } from './components/projects/add-contributor/add-contributor.component';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';

const appRoutes = [
  { path: '', component: BugComponent },
  {
    path: 'bugs/edit/:id',
    component: BugEditPageComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: ['admin'] },
  },
  {
    path: 'bugs/report',
    component: BugReportComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: ['admin'] },
  },
  {
    path: 'bugs/report/:projectId',
    component: BugReportComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: ['admin'] },
  },
  { path: 'bugs/:id', component: BugPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'projects', component: ProjectsComponent },
  {
    path: 'projects/add-contributor/:projectId',
    component: AddContributorComponent,
  },
  { path: 'projects/:id', component: ProjectPageComponent },
  {
    path: 'admin',
    component: UserComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: ['admin'] },
  },
  {
    path: 'profile',
    component: MeComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'profile/edit',
    component: EditProfileComponent,
    canActivate: [AuthenticationGuard],
  },
];

@NgModule({
  declarations: [
    AppComponent,
    AsideComponent,
    MainComponent,
    BugComponent,
    UserComponent,
    NavbarComponent,
    NavPromptComponent,
    LoginComponent,
    RegisterComponent,
    MeComponent,
    BugArticleComponent,
    BugPageComponent,
    BugEditPageComponent,
    BugReportComponent,
    ProjectsComponent,
    ProjectArticleComponent,
    ProjectPageComponent,
    ProjectEditPageComponent,
    AddContributorComponent,
    EditProfileComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes, { useHash: true }),
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
  ],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    AuthorizationGuard,
    AuthenticationGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
