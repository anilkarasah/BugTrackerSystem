import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';

import { AppComponent } from './app.component';
import { AsideComponent } from './components/aside-component/aside-component.component';
import { MainComponent } from './components/main-component/main-component.component';
import { AdminComponent } from './components/admin/admin.component';
import { NavbarComponent } from './components/aside-component/navbar/navbar.component';
import { ProfileComponent } from './components/profile/profile.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthInterceptor } from './guards/AuthInterceptor';
import { AuthorizationGuard } from './guards/authorization.guard';
import { AuthenticationGuard } from './guards/authentication.guard';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';
import { AppRouteModule } from './app-route.module';
import { ProjectsModule } from './components/projects/projects.module';
import { AuthModule } from './components/auth/auth.module';
import { BugsModule } from './components/bugs/bugs.module';
import { HeaderModule } from './components/header/header.module';

@NgModule({
  declarations: [
    AppComponent,
    AsideComponent,
    MainComponent,
    AdminComponent,
    NavbarComponent,
    ProfileComponent,
    EditProfileComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    AppRouteModule,
    ProjectsModule,
    AuthModule,
    BugsModule,
    HeaderModule,
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
