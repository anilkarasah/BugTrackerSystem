import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ProfileRouteModule } from './profile-route.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ProfileComponent, EditProfileComponent],
  imports: [CommonModule, FontAwesomeModule, FormsModule, ProfileRouteModule],
})
export class ProfileModule {}
