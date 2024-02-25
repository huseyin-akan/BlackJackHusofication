import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {

  constructor(private toastr: ToastrService) { }

  showSuccess(message: string,title: string="") {
    this.toastr.success(title, message);
  }

  showError(message: string, title: string  ="") {
    this.toastr.error(title, message);
  }
}
