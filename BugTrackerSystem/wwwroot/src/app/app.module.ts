import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AsideComponent } from './components/aside-component/aside-component.component';
import { MainComponent } from './components/main-component/main-component.component';
import { ButtonComponent } from './components/button/button.component';
import { BugComponent } from './components/bugs/bug.component';
import { UserComponent } from './components/users/user-component.component';
import { NavbarComponent } from './components/aside-component/navbar/navbar.component';
import { NavPromptComponent } from './components/aside-component/nav-prompt/nav-prompt.component';
import { LoginComponent } from './components/auth/login/login.component';
import { LogoutComponent } from './components/auth/logout/logout.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { MeComponent } from './components/profile/me/me.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BugArticleComponent } from './components/bugs/bug-article/bug-article.component';

const appRoutes = [
  { path: '', component: BugComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  // { path: 'projects', component: ProjectComponent},
  { path: 'users', component: UserComponent },
  { path: 'me', component: MeComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    AsideComponent,
    MainComponent,
    ButtonComponent,
    BugComponent,
    UserComponent,
    NavbarComponent,
    NavPromptComponent,
    LoginComponent,
    LogoutComponent,
    RegisterComponent,
    MeComponent,
    BugArticleComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    FormsModule,
    FontAwesomeModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
