import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';
import { ToastrModule } from 'ngx-toastr';

// Module imports
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppRouteModule } from './app-route.module';
import { ProjectsModule } from './components/projects/projects.module';
import { AuthModule } from './components/auth/auth.module';
import { BugsModule } from './components/bugs/bugs.module';
import { ProfileModule } from './components/profile/profile.module';

// Component imports
import { AppComponent } from './app.component';
import { AsideComponent } from './components/aside-component/aside-component.component';
import { AdminComponent } from './components/admin/admin.component';
import { NavbarComponent } from './components/aside-component/navbar/navbar.component';

// Interceptor imports
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';

// Guard imports
import { AuthorizationGuard } from './guards/authorization.guard';
import { AuthenticationGuard } from './guards/authentication.guard';

@NgModule({
  declarations: [AppComponent, AsideComponent, AdminComponent, NavbarComponent],
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
    ProfileModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
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
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    AuthorizationGuard,
    AuthenticationGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
