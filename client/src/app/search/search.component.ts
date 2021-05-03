import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import {
  ModalDismissReasons,
  NgbDateParserFormatter,
  NgbDateStruct,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { UserService } from '../_services/user/user.service';
import * as Capacity from '../_models/capacity';
import { ManufacturingProcess } from '../_models/manufacturingProcess';
import { HttpClient } from '@angular/common/http';
import { SearchCriteriaSpareCapacity } from '../_models/data-table';
import { Subject, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { LookUpService } from '../_services/look-up/look-up.service';
import { SearchService } from '../_services/search/search.service';

@Component({
  selector: 'app-capacity-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit {
  locationId: number = 0;
  searchCriteria: SearchCriteriaSpareCapacity = {
    isPageLoad: false,
    industryId: null,
    processId: null,
    city: null,
    startDate: null,
    endDate: null,
  };
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  userInfo: any;
  IndustryTypes: any = [];
  ManufacturingProcessesModel: ManufacturingProcess[] = [];
  cityList: string[] = [];
  SpareCapacitiesModel: Capacity.SpareCapacities[] = [];
  SpareCapacityModel: Capacity.SpareCapacities = new Capacity.SpareCapacities();
  industryTypeId: number = 0;
  ManufacturingProcessId: number = 0;
  cityName: string = '';
  startDate: string;
  endDate: string;
  machineCapacityStartDate: NgbDateStruct;
  machineCapacityEndDate: NgbDateStruct;
  manufacturerLocationsVM: string[] = [];
  cityListVM: string[] = [];
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  timerSubscription: Subscription;
  closeResult: string;
  selectedOrdersIds: any[] = [];

  constructor(
    private userService: UserService,
    private searchService: SearchService,
    private toastr: ToastrService,
    private lookUpService: LookUpService,
    private router: Router,
    private ngbDateParserFormatter: NgbDateParserFormatter
  ) {}

  ngOnInit(): void {
    this.loadIndustries();
    this.GetUserManufacturingProcessForDropDown();
    this.loadUserInfo();
    this.getSearchViewCities();
    this.getCapacitySearchResult();
  }

  storeOrderIdChange(event, entry_no) {
    if (event.target.checked) {
      this.selectedOrdersIds.push(entry_no);
    } else {
      this.selectedOrdersIds = this.selectedOrdersIds.filter(
        (rowId) => rowId !== entry_no
      );
    }
  }

  filterTable() {
    this.searchCriteria.industryId = this.industryTypeId;
    this.searchCriteria.processId = this.ManufacturingProcessId;
    this.searchCriteria.city = this.cityName;
    this.searchCriteria.startDate = this.startDate;
    this.searchCriteria.endDate = this.endDate;
    if (
      (this.startDate == undefined && this.endDate != undefined) ||
      (this.startDate != undefined && this.endDate == undefined)
    ) {
      this.toastr.error(
        'Start date and End date both are required in order to search!'
      );
    } else {
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        dtInstance.ajax.reload();
      });
    }
  }
  isSelected(id: number) {
    return this.selectedOrdersIds.some((rowId) => rowId == id);
  }
  onSelectStartDate(date: NgbDateStruct) {
    if (date != null) {
      this.machineCapacityStartDate = date;
      this.startDate = this.ngbDateParserFormatter.format(date);
    }
  }

  clearOrderSelection() {
    this.selectedOrdersIds = [];
  }

  createOrder() {
    if (this.selectedOrdersIds?.length < 1) {
      this.toastr.error('Please select order first!');
    } else {
      let objToSend: NavigationExtras = {
        queryParams: {
          ids: this.selectedOrdersIds,
          isShow: true,
        },
      };
      this.router.navigate(['/order-confirmation'], {
        state: { productdetails: objToSend },
      });
    }
  }

  onSelectEndDate(date: NgbDateStruct) {
    if (date != null) {
      this.machineCapacityEndDate = date;
      this.endDate = this.ngbDateParserFormatter.format(date);
    }
  }

  GetUserManufacturingProcessForDropDown() {
    this.userService
      .GetUserManufacturingProcessForDropDown()
      .subscribe((resp) => {
        this.ManufacturingProcessesModel = resp;
      });
  }

  loadIndustries() {
    this.lookUpService.getIndustries().subscribe(
      (response) => {
        this.IndustryTypes = response;
      },
      (error) => {
        this.toastr.error('Unable to load industries!');
        console.log(error);
      }
    );
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

  getSearchViewCities() {
    this.searchService.GetSearchViewCities().subscribe(
      (response) => {
        this.cityListVM = response;
      },
      (error) => {
        this.toastr.error('Unable to load cities!');
        console.log(error);
      }
    );
  }

  getCapacitySearchResult() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      ajax: (dataTablesParameters: any, callback) => {
        dataTablesParameters.SearchCriteriaSpareCapacity = this.searchCriteria;
        this.searchService
          .FilterSpareCapacity(dataTablesParameters)
          .subscribe((resp) => {
            that.SpareCapacitiesModel = resp.data;
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

  onSelectDate(date: NgbDateStruct) {
    if (date != null) {
      this.machineCapacityStartDate = date;
    }
  }
}
