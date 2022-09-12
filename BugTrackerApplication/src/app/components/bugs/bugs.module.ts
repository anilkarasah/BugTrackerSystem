import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugArticleModule } from './bug-article/bug-article.module';
import { BugEditPageModule } from './bug-edit-page/bug-edit-page.module';
import { BugListModule } from './bug-list/bug-list.module';
import { BugPageModule } from './bug-page/bug-page.module';
import { BugReportModule } from './bug-report/bug-report.module';
import { RouterModule } from '@angular/router';
import { BugsComponent } from './bugs.component';

@NgModule({
  declarations: [BugsComponent],
  imports: [
    CommonModule,
    RouterModule,
    BugArticleModule,
    BugEditPageModule,
    BugListModule,
    BugPageModule,
    BugReportModule,
  ],
  exports: [BugsComponent],
})
export class BugsModule {}
