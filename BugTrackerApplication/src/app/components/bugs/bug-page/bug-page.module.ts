import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugPageComponent } from './bug-page.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [BugPageComponent],
  imports: [CommonModule, RouterModule, FormsModule],
  exports: [BugPageComponent],
})
export class BugPageModule {}
