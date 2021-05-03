import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../_services/user/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
})
export class LandingComponent implements OnInit {
  userInfo: any;

  constructor(
    private router: Router,
    public userService: UserService,
    public toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
  }

  searchClick() {
    this.router.navigateByUrl('/search');
  }

  sellClick() {
    this.router.navigateByUrl('/locations');
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
