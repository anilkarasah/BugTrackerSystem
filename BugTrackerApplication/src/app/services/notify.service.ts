import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class NotifyService {
  constructor(private toastr: ToastrService, private router: Router) {}

  alertSuccess(
    successMessage: string,
    locateTimeout?: number,
    locateUrl?: string | null
  ): void {
    if (locateTimeout && locateTimeout > 0) {
      setTimeout(() => {
        if (locateUrl === null) window.location.reload();
        else if (locateUrl) this.router.navigate([locateUrl]);
        else this.router.navigate(['/']);
      }, locateTimeout);
    }

    this.toastr.success(successMessage);
  }

  alertError(
    errorMessage: string,
    errorTitle?: string,
    locateTimeout?: number,
    locateUrl?: string | null
  ) {
    if (locateTimeout && locateTimeout > 0) {
      setTimeout(() => {
        if (locateUrl === null) window.location.reload();
        else if (locateUrl) this.router.navigate([locateUrl]);
        else this.router.navigate(['/']);
      }, locateTimeout);
    }

    this.toastr.error(errorMessage, errorTitle);
  }
}
