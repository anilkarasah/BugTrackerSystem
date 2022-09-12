import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectEditPageComponent } from './project-edit-page.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ProjectEditPageComponent],
  imports: [CommonModule, RouterModule, FormsModule],
  exports: [ProjectEditPageComponent],
})
export class ProjectEditPageModule {}
