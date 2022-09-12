import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugArticleComponent } from './bug-article.component';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  declarations: [BugArticleComponent],
  imports: [CommonModule, RouterModule, FontAwesomeModule],
  exports: [BugArticleComponent],
})
export class BugArticleModule {}
