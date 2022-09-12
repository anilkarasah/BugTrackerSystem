import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugEditPageComponent } from './bug-edit-page.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [BugEditPageComponent],
  imports: [CommonModule, FormsModule, RouterModule],
  exports: [BugEditPageComponent],
})
export class BugEditPageModule {}
