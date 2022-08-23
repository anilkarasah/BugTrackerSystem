import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

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

const appRoutes = [
  { path: '', component: BugComponent },
  { path: 'bugs/:id', component: BugPageComponent },
  { path: 'bugs/:id/edit', component: BugEditPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  // { path: 'projects', component: ProjectComponent },
  { path: 'admin', component: UserComponent },
  { path: 'profile', component: MeComponent },
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
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes, { useHash: true }),
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
