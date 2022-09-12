import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugReportComponent } from './bug-report.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [BugReportComponent],
  imports: [CommonModule, FormsModule, RouterModule],
  exports: [BugReportComponent],
})
export class BugReportModule {}
