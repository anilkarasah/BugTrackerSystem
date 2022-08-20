import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AsideComponentComponent } from './components/aside-component/aside-component.component';
import { MainComponentComponent } from './components/main-component/main-component.component';
import { ButtonComponent } from './components/button/button.component';
import { BugComponentComponent } from './components/bugs/bug-component/bug-component.component';
import { UserComponentComponent } from './components/users/user-component/user-component.component';
import { NavbarComponent } from './components/aside-component/navbar/navbar.component';
import { NavPromptComponent } from './components/aside-component/nav-prompt/nav-prompt.component';

@NgModule({
  declarations: [
    AppComponent,
    AsideComponentComponent,
    MainComponentComponent,
    ButtonComponent,
    BugComponentComponent,
    UserComponentComponent,
    NavbarComponent,
    NavPromptComponent,
  ],
  imports: [BrowserModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
