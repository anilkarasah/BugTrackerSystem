import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateProjectComponent } from './create-project.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [CreateProjectComponent],
  imports: [CommonModule, RouterModule, FormsModule],
  exports: [CreateProjectComponent],
})
export class CreateProjectModule {}
