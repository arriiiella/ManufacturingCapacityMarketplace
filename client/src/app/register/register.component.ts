import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account/account.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Router } from '@angular/router';
import { LookUpService } from '../_services/look-up/look-up.service';
import { Register } from '../_models/register';
import { Customer } from '../_models/customer';
import { Address } from '../_models/address';
import * as ManufacturerInfo from '../_models/manufacturer';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  IndustryTypes: any = [];
  countries: any = [];
  RegisterModel: Register = new Register();
  CustomerModel: Customer = new Customer();
  ManufacturerModel: ManufacturerInfo.Manufacturer = new ManufacturerInfo.Manufacturer();
  AddressModel: Address = new Address();
  AddressesModel: Address[] = [];
  step = 1;
  constructor(
    private accountService: AccountService,
    private LookUpService: LookUpService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private router: Router,
    private changeRef: ChangeDetectorRef,
    private ngxService: NgxUiLoaderService
  ) {}

  ngOnInit(): void {
    this.loadIndustries();
    this.loadCountries();
    this.CustomerModel.IsPurchaseCapacity = true;
  }

  loadIndustries() {
    this.LookUpService.getIndustries().subscribe(
      (response) => {
        this.IndustryTypes = response;
      },
      (error) => {
        this.toastr.error('Unable to load industries!');
        console.log(error);
      }
    );
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

  setUsage(usage) {
    if (usage == 'Sell Capacity') {
      this.CustomerModel.IsSellCapacity = true;
      this.CustomerModel.IsPurchaseCapacity = false;
      this.CustomerModel.UseBothCapacity = false;
    } else if (usage == 'Purchase Capacity') {
      this.CustomerModel.IsSellCapacity = false;
      this.CustomerModel.IsPurchaseCapacity = true;
      this.CustomerModel.UseBothCapacity = false;
    } else {
      this.CustomerModel.IsSellCapacity = true;
      this.CustomerModel.IsPurchaseCapacity = true;
      this.CustomerModel.UseBothCapacity = true;
    }
  }

  openAddress(content) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false,
    };
    this.modalService.open(content, ngbModalOptions);
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
      this.AddressModel.Id =
        this.AddressesModel == undefined ? 1 : this.AddressesModel.length + 1;
      this.AddressesModel.push(this.AddressModel);
      this.changeRef.detectChanges();
      this.AddressModel = new Address();
      this.modalService.dismissAll();
    }
  }

  register(request) {
    this.ngxService.start();
    this.accountService.register(request).subscribe(
      (response) => {
        this.ngxService.stop();
        this.toastr.success('You have successfully registered');
        this.router.navigate(['/login']);
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

  completeStep1() {
    if (
      !this.RegisterModel.FirstName ||
      this.RegisterModel.FirstName.trim().length < 1
    ) {
      this.toastr.error('First name is required!');
    } else if (
      !this.RegisterModel.LastName ||
      this.RegisterModel.LastName.trim().length < 1
    ) {
      this.toastr.error('Last name is required!');
    } else if (
      !this.RegisterModel.UserName ||
      this.RegisterModel.UserName.trim().length < 1
    ) {
      this.toastr.error('User name is required!');
    } else if (
      !this.RegisterModel.Email ||
      this.RegisterModel.Email.trim().length < 1
    ) {
      this.toastr.error('Email is required!');
    } else if (!this.RegisterModel.Password) {
      this.toastr.error('Password is required!');
    } else if (!this.RegisterModel.CPassword) {
      this.toastr.error('Confirm password is required!');
    } else if (this.RegisterModel.Password != this.RegisterModel.CPassword) {
      this.toastr.error('Password does not match!');
    } else {
      this.step = 2;
    }
  }

  completeStep2() {
    if (!this.CustomerModel.Name || this.CustomerModel.Name.trim().length < 1) {
      this.toastr.error('Business Name is required!');
    } else if (
      !this.CustomerModel.IndustryId ||
      this.CustomerModel.IndustryId == 0
    ) {
      this.toastr.error('Industry Type is required!');
    } else if (
      !this.CustomerModel.AddressId ||
      this.CustomerModel.AddressId == 0
    ) {
      this.toastr.error('Address is required!');
    } else if (
      !this.CustomerModel.VatRegistrationNo ||
      this.CustomerModel.VatRegistrationNo.trim().length < 1
    ) {
      this.toastr.error('VAT is required!');
    } else if (
      !this.CustomerModel.BillingAddressId ||
      this.CustomerModel.BillingAddressId == 0
    ) {
      this.toastr.error('Billing Address is required!');
    } else {
      if (
        this.CustomerModel.IsPurchaseCapacity &&
        this.CustomerModel.IsSellCapacity == false
      ) {
        let request = {
          User: this.RegisterModel,
          customer: this.CustomerModel,
          Addresses: this.AddressesModel,
          Manufacturer: null,
        };
        this.register(request);
      } else {
        this.step = 3;
      }
    }
  }

  completeStep3() {
    if (
      !this.ManufacturerModel.Name ||
      this.ManufacturerModel.Name.trim().length < 1
    ) {
      this.toastr.error('Business Name is required!');
    } else if (
      !this.ManufacturerModel.IndustryId ||
      this.ManufacturerModel.IndustryId == 0
    ) {
      this.toastr.error('Industry Type is required!');
    } else if (
      !this.ManufacturerModel.AddressId ||
      this.ManufacturerModel.AddressId == 0
    ) {
      this.toastr.error('Address is required!');
    } else if (
      !this.ManufacturerModel.VatRegistrationNo ||
      this.ManufacturerModel.VatRegistrationNo.trim().length < 1
    ) {
      this.toastr.error('VAT is required!');
    } else if (
      !this.ManufacturerModel.BillingAddressId ||
      this.ManufacturerModel.BillingAddressId == 0
    ) {
      this.toastr.error('Billing Address is required!');
    } else {
      let request = {
        User: this.RegisterModel,
        customer: this.CustomerModel,
        Addresses: this.AddressesModel,
        Manufacturer: this.ManufacturerModel,
      };
      this.register(request);
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
