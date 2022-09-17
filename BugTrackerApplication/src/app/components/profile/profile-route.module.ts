import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile.component';
import { RouterModule } from '@angular/router';
import { AuthenticationGuard } from 'src/app/guards/authentication.guard';

const childrenRoutes = [
  {
    path: '',
    component: ProfileComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: ':userId',
    component: ProfileComponent,
  },
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(childrenRoutes)],
  exports: [RouterModule],
})
export class ProfileRouteModule {}
