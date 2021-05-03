import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account/account.service';
import { UserService } from '../_services/user/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  userInfo: any;
  isCollapsed: boolean;

  constructor(
    public accountService: AccountService,
    private router: Router,
    public userService: UserService,
    public toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
  }
  logout() {
    this.accountService.logout();
    this.router.navigate(['/']);
  }

  loadUserInfo() {
    this.userService.GetLoggedInUserInformation().subscribe(
      (response) => {
        this.userInfo = response;
      },
      (error) => {
        this.toastr.error('Unable to load user information!');
        console.log(error);
      }
    );
  }
}
