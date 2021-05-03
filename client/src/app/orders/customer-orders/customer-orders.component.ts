import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import {
  NgbDateParserFormatter,
  NgbDateStruct,
  NgbTimeStruct,
} from '@ng-bootstrap/ng-bootstrap';
import { DataTableDirective } from 'angular-datatables';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subject } from 'rxjs';
import { OrderService } from 'src/app/_services/order/order.service';
import { SearchCriteria } from '../../_models/data-table';
import * as Order from '../../_models/order';
import { UserService } from '../../_services/user/user.service';

@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.css'],
})
export class CustomerOrdersComponent implements OnInit {
  machineCapacityEntryDate: NgbDateStruct;
  machineCapacityEntryTime: NgbTimeStruct;
  searchCriteria: SearchCriteria = { isPageLoad: false, filter: '' };
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  OrdersModel: Order.Orders[] = [];
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  userInfo: any;
  constructor(
    private ngbDateParserFormatter: NgbDateParserFormatter,
    private _Activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private userService: UserService,
    private orderService: OrderService,
    private ngxService: NgxUiLoaderService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
    this.loadCustomerOrders();
  }

  calculateTotal(order) {
    return order.orderLines
      .map((x) => x.lineAmount)
      .reduce(function (acc, cur) {
        return acc + cur;
      });
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

  viewOrder(orderNo) {
    let objToSend: NavigationExtras = {
      queryParams: {
        ids: [orderNo],
        isShow: false,
      },
    };
    this.router.navigate(['/order-confirmation'], {
      state: { productdetails: objToSend },
    });
  }

  loadCustomerOrders() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      ajax: (dataTablesParameters: any, callback) => {
        dataTablesParameters.searchCriteria = this.searchCriteria;
        this.orderService
          .GetCustomerOrders(dataTablesParameters)
          .subscribe((resp) => {
            that.OrdersModel = resp.data;
            console.log(resp.data);
            callback({
              recordsTotal: resp.recordsTotal,
              recordsFiltered: resp.recordsFiltered,
              data: [],
            });
          });
      },
      columns: [{ data: 'id' }],
    };
  }
}
