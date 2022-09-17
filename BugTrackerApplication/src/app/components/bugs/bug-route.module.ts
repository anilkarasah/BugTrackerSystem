import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BugListComponent } from './bug-list/bug-list.component';
import { BugReportComponent } from './bug-report/bug-report.component';
import { AuthorizationGuard } from 'src/app/guards/authorization.guard';
import { BugPageComponent } from './bug-page/bug-page.component';
import { BugEditPageComponent } from './bug-edit-page/bug-edit-page.component';
import { RouterModule } from '@angular/router';

const childrenRoutes = [
  {
    path: '',
    component: BugListComponent,
  },
  {
    path: 'report',
    component: BugReportComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
  },
  {
    path: ':bugId',
    component: BugPageComponent,
  },
  {
    path: ':bugId/edit',
    component: BugEditPageComponent,
    canActivate: [AuthorizationGuard],
    data: { roles: 'admin' },
  },
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(childrenRoutes)],
  exports: [RouterModule],
})
export class BugRouteModule {}
