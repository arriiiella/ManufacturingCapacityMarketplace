import { Component, OnInit, ViewChild } from '@angular/core';
import {
  ModalDismissReasons,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { UserService } from '../_services/user/user.service';
import { LocationService } from '../_services/location/location.service';
import { Address } from '../_models/address';
import { ManufacturingLocation } from '../_models/location';
import { SearchCriteria } from '../_models/data-table';
import { Subject, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { LookUpService } from '../_services/look-up/look-up.service';

@Component({
  selector: 'app-manufacturer-location',
  templateUrl: './manufacturer-location.component.html',
  styleUrls: ['./manufacturer-location.component.css'],
})
export class ManufacturerLocationComponent implements OnInit {
  countries: any = [];
  ManufacturingLocationModel: ManufacturingLocation = new ManufacturingLocation();
  searchCriteria: SearchCriteria = { isPageLoad: false, filter: '' };
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  AddressesModel: Address[] = [];
  ManufacturingLocationsModel: ManufacturingLocation[] = [];
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  timerSubscription: Subscription;
  AddressModel: Address = new Address();
  closeResult: string;

  constructor(
    private userService: UserService,
    private locationService: LocationService,
    private LookUpService: LookUpService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private ngxService: NgxUiLoaderService
  ) {}

  ngOnInit(): void {
    this.loadCountries();
    this.loadUserAddress();
    this.getUserManufacturingLocation();
  }

  loadCountries() {
    this.LookUpService.getCountries().subscribe(
      (response) => {
        this.countries = response;
      },
      (error) => {
        this.toastr.error('Unable to load countries!');
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

  getUserManufacturingLocation() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      ajax: (dataTablesParameters: any, callback) => {
        dataTablesParameters.searchCriteria = this.searchCriteria;
        this.locationService
          .getUserManufacturingLocation(dataTablesParameters)
          .subscribe((resp) => {
            console.log(resp.data);
            that.ManufacturingLocationsModel = resp.data;
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

  saveLocation() {
    if (
      !this.ManufacturingLocationModel.Name ||
      this.ManufacturingLocationModel.Name.trim().length < 1
    ) {
      this.toastr.error('Location name is required!');
    } else if (
      !this.ManufacturingLocationModel.AddressId ||
      this.ManufacturingLocationModel.AddressId == 0
    ) {
      this.toastr.error('Please select address');
    } else {
      this.ngxService.start();
      this.locationService
        .AddUserLocation(this.ManufacturingLocationModel)
        .subscribe(
          (response) => {
            this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
              dtInstance.ajax.reload();
            });
            this.ngxService.stop();
            this.toastr.success('Location is successfully added');
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
      this.AddressModel = new Address();
    }
  }

  openAddress(content) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false,
    };
    this.modalService.open(content, ngbModalOptions);
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  rerender(): void {
    this.searchCriteria.isPageLoad = false;
    this.searchCriteria.filter = '';
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }

  search() {
    this.rerender();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();

    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
  }

  private refreshData(): void {
    this.rerender();
    this.subscribeToData();
  }

  private subscribeToData(): void {
    this.refreshData();
  }

  saveAddress() {
    if (!this.AddressModel.Name || this.AddressModel.Name.trim().length < 1) {
      this.toastr.error('Name is required!');
    } else if (
      !this.AddressModel.Address1 ||
      this.AddressModel.Address1.trim().length < 1
    ) {
      this.toastr.error('Address line 1 is required!');
    } else if (
      !this.AddressModel.City ||
      this.AddressModel.City.trim().length < 1
    ) {
      this.toastr.error('City is required!');
    } else if (
      !this.AddressModel.County ||
      this.AddressModel.County.trim().length < 1
    ) {
      this.toastr.error('County is required!');
    } else if (
      !this.AddressModel.CountryCode ||
      this.AddressModel.CountryCode.trim().length < 1
    ) {
      this.toastr.error('Country Code is required!');
    } else if (
      !this.AddressModel.Telephone ||
      this.AddressModel.Telephone.trim().length < 1
    ) {
      this.toastr.error('Telephone is required!');
    } else if (
      !this.AddressModel.Postcode ||
      this.AddressModel.Postcode.trim().length < 1
    ) {
      this.toastr.error('Postcode is required!');
    } else if (
      !this.AddressModel.Email ||
      this.AddressModel.Email.trim().length < 1
    ) {
      this.toastr.error('Email is required!');
    } else {
      this.ngxService.start();
      this.userService.AddAddress(this.AddressModel).subscribe(
        (response) => {
          this.loadUserAddress();
          this.ngxService.stop();
          this.toastr.success('Address is successfully added');
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
      this.AddressModel = new Address();
      this.modalService.dismissAll();
    }
  }

  deleteLocation(id) {
    this.ngxService.start();
    let model = {
      Id: id,
    };
    this.locationService.DeleteUserLocation(model).subscribe(
      (response) => {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
          dtInstance.ajax.reload();
        });
        this.ngxService.stop();
        this.toastr.success('Location is successfully deleted');
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
    this.AddressModel = new Address();
    this.modalService.dismissAll();
  }

  deletePopup(content, locationId) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
          if (result === 'yes') {
            this.deleteLocation(locationId);
          }
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
