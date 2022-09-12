import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectArticleComponent } from './project-article.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [CommonModule, FontAwesomeModule, RouterModule],
  declarations: [ProjectArticleComponent],
  exports: [ProjectArticleComponent],
})
export class ProjectArticleModule {}
