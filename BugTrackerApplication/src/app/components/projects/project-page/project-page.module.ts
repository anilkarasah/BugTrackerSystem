import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectPageComponent } from './project-page.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ProjectPageComponent],
  imports: [CommonModule, FontAwesomeModule, RouterModule, FormsModule],
  exports: [ProjectPageComponent],
})
export class ProjectPageModule {}
