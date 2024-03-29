import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugListComponent } from './bug-list.component';
import { BugArticleModule } from '../bug-article/bug-article.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [BugListComponent],
  imports: [CommonModule, RouterModule, BugArticleModule],
  exports: [BugListComponent],
})
export class BugListModule {}
