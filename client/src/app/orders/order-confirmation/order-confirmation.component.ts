import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { UserService } from '../../_services/user/user.service';
import { OrderService } from 'src/app/_services/order/order.service';

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.css'],
})
export class OrderConfirmationComponent implements OnInit {
  detailProduct: any;
  orderInfo: any[] = [];
  todayDate: Date = new Date();
  userInfo: any;
  ids: string = '';
  isShow = true;
  constructor(
    private userService: UserService,
    private orderService: OrderService,
    private toastr: ToastrService,
    private router: Router,
    private ngxService: NgxUiLoaderService
  ) {
    this.detailProduct = this.router.getCurrentNavigation().extras.state;
    if (this.detailProduct == undefined) {
      this.router.navigate(['/capacity-search']);
    }
    this.ids = this.detailProduct.productdetails.queryParams.ids
      .map((x) => x)
      .join(',');
    this.isShow = this.detailProduct.productdetails.queryParams.isShow;
    debugger;
    if (this.isShow) {
      this.loadOrderInformation(this.ids);
      this.loadUserInfo();
    } else {
      this.loadOrderInformationByOrderNo(parseInt(this.ids));
    }
  }

  loadUserInfo() {
    this.userService.GetLoggedInUserInformation().subscribe(
      (response) => {
        this.userInfo = response;
      },
      (error) => {
        console.log(error);
        this.toastr.error('Unable to load user information!');
      }
    );
  }

  loadOrderInformation(ids) {
    let model = {
      CapacityEntryNumbers: ids,
    };
    this.orderService.GetOrderConfirmationDetails(model).subscribe(
      (response) => {
        console.log(response);
        this.orderInfo = response;
      },
      (error) => {
        console.log(error);
        this.toastr.error('Unable to load order details!');
      }
    );
  }

  ngOnInit(): void {}

  loadOrderInformationByOrderNo(no) {
    let model = {
      OrderNo: no,
    };
    this.orderService.GetOrderByOrderNo(model).subscribe(
      (response) => {
        this.orderInfo = response;
      },
      (error) => {
        console.log(error);
        this.toastr.error('Unable to load order details!');
      }
    );
  }

  confirmOrder() {
    if (this.ids.length == 0) {
      this.toastr.error('Invalid order selection!');
    } else {
      this.ngxService.start();
      let model = {
        CapacityEntryNumbers: this.ids,
      };
      this.orderService.CreateOrder(model).subscribe(
        (response) => {
          this.ngxService.stop();
          if (response) {
            this.toastr.success('Order is successfully created.');
            this.router.navigate(['/landing']);
          } else {
            this.toastr.error(
              'An error has occured, Please contact system admin'
            );
          }
        },
        (error) => {
          this.ngxService.stop();
          console.log(error);
          this.toastr.error('Unable to load order details!');
        }
      );
    }
  }
}
