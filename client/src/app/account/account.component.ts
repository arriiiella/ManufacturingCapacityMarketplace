import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../_services/user/user.service';
import { Address} from '../_models/address';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-profile',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
export class AccountComponent implements OnInit {
  userInfo: any;
  AddressesModel: Address[] = [];

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private ngxService: NgxUiLoaderService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
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

  loadUserAddress() {
    this.userService.getUserAddresses().subscribe(
      (response) => {
        console.log(response);
        this.AddressesModel = response;
      },
      (error) => {
        console.log(error);
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
