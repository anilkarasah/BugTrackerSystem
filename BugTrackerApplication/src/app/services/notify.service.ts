import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class NotifyService {
  constructor(private toastr: ToastrService) {}

  alertSuccess(
    successMessage: string,
    locateTimeout?: number,
    locateUrl?: string | null
  ): void {
    if (locateTimeout && locateTimeout > 0) {
      setTimeout(() => {
        if (locateUrl === null) window.location.reload();
        else if (locateUrl) window.location.replace(locateUrl);
        else window.location.replace('/');
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
        else if (locateUrl) window.location.replace(locateUrl);
        else window.location.replace('/');
      }, locateTimeout);
    }

    this.toastr.error(errorMessage, errorTitle);
  }
}
