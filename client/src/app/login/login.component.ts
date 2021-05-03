import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { AccountService } from '../_services/account/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  model: any = {};
  constructor(
    public accountService: AccountService,
    private ngxService: NgxUiLoaderService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.accountService.currentUser$) {
      this.router.navigate(['/login']);
    }
  }

  login() {
    if (!this.model.username || this.model.username.trim().length < 1) {
      this.toastr.error('Username is required');
    } else if (!this.model.password || this.model.password.trim().length < 1) {
      this.toastr.error('Password is required');
    } else {
      this.ngxService.start();
      this.accountService.login(this.model).subscribe(
        (response) => {
          this.ngxService.stop();
          if (response != undefined) {
            this.toastr.error('Invalid username or password');
          } else {
            this.router.navigate(['/landing']);
          }
        },
        (error) => {
          this.ngxService.stop();
          if (error != undefined) {
            this.toastr.error(error.error);
          } else {
            this.toastr.error(
              'An error has occured, Please contact system admin!'
            );
          }
        }
      );
    }
  }
}
