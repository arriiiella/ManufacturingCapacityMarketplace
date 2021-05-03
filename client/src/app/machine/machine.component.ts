import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ModalDismissReasons,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { UserService } from '../_services/user/user.service';
import { Address } from '../_models/address';
import { ManufacturingLocation } from '../_models/location';
import { Machine } from '../_models/machine';
import { ManufacturingProcess } from '../_models/manufacturingProcess';
import { HttpClient } from '@angular/common/http';
import { SearchCriteria } from '../_models/data-table';
import { Subject, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { MachineService } from '../_services/machine/machine.service';
import { LocationService } from '../_services/location/location.service';

@Component({
  selector: 'app-machine',
  templateUrl: './machine.component.html',
  styleUrls: ['./machine.component.css'],
})
export class MachineComponent implements OnInit {
  locationId: number = 0;
  searchCriteria: SearchCriteria = { isPageLoad: false, filter: '' };
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  AddressesModel: Address[] = [];
  ManufacturingLocationsModel: ManufacturingLocation[] = [];
  ManufacturingProcessesModel: ManufacturingProcess[] = [];
  MachinesModel: Machine[] = [];
  MachineModel: Machine = new Machine();
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  timerSubscription: Subscription;
  closeResult: string;

  constructor(
    private userService: UserService,
    private machineService: MachineService,
    private locationService: LocationService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private http: HttpClient,
    private router: Router,
    private changeRef: ChangeDetectorRef,
    private ngxService: NgxUiLoaderService,
    private _Activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this._Activatedroute.paramMap.subscribe((params) => {
      let id = params.get('id');
      this.locationId = Number(id);
      this.searchCriteria.filter = id;
    });

    this.getUserManufacturingLocationForDropDown();
    this.GetUserManufacturingProcessForDropDown();
    this.getUserMachines();
  }

  reloadTableForLocationChange() {
    this.searchCriteria.filter = this.locationId.toString();
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.ajax.reload();
    });
  }

  getUserManufacturingLocationForDropDown() {
    this.locationService
      .getUserManufacturingLocationForDropDown()
      .subscribe((resp) => {
        this.ManufacturingLocationsModel = resp;
      });
  }

  GetUserManufacturingProcessForDropDown() {
    this.userService
      .GetUserManufacturingProcessForDropDown()
      .subscribe((resp) => {
        this.ManufacturingProcessesModel = resp;
      });
  }

  getUserMachines() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      ajax: (dataTablesParameters: any, callback) => {
        dataTablesParameters.searchCriteria = this.searchCriteria;
        this.machineService
          .GetUserMachines(dataTablesParameters)
          .subscribe((resp) => {
            that.MachinesModel = resp.data;
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

  openMachinePopup(content) {
    if (!this.locationId || this.locationId == 0) {
      this.toastr.error('Please select location!');
    } else {
      let ngbModalOptions: NgbModalOptions = {
        backdrop: 'static',
        keyboard: false,
      };
      this.modalService.open(content, ngbModalOptions);
    }
  }

  clickAddLocation() {
    this.router.navigateByUrl('/locations');
  }

  deletePopup(content, machineId) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
          if (result === 'yes') {
            this.deleteMachine(machineId);
          }
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  deleteMachine(id) {
    this.ngxService.start();
    let model = {
      Id: id,
    };
    this.machineService.DeleteUserMachine(model).subscribe(
      (response) => {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
          dtInstance.ajax.reload();
        });
        this.ngxService.stop();
        this.toastr.success('Machine is successfully deleted');
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
    this.MachineModel = new Machine();
    this.modalService.dismissAll();
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

  saveMachine() {
    if (!this.MachineModel.Name || this.MachineModel.Name.trim().length < 1) {
      this.toastr.error('Machine name is required!');
    } else if (
      !this.MachineModel.ModelNo ||
      this.MachineModel.ModelNo.trim().length < 1
    ) {
      this.toastr.error('Model no is required!');
    } else if (!this.MachineModel.Capacity) {
      this.toastr.error('Capacity is required!');
    } else if (!this.MachineModel.SetupTime) {
      this.toastr.error('Setup Time is required!');
    } else if (!this.MachineModel.Capacity) {
      this.toastr.error('Capacity is required!');
    } else if (!this.MachineModel.CostPerUnit) {
      this.toastr.error('Cost Per Unit is required!');
    } else if (
      !this.MachineModel.ManufacturingProcessId ||
      this.MachineModel.ManufacturingProcessId == 0
    ) {
      this.toastr.error('Please select process!');
    } else {
      this.ngxService.start();
      this.MachineModel.LocationId = this.locationId;
      this.machineService.AddMachine(this.MachineModel).subscribe(
        (response) => {
          this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            dtInstance.ajax.reload();
          });
          this.ngxService.stop();
          this.toastr.success('Machine is successfully added');
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
      this.MachineModel = new Machine();
      this.modalService.dismissAll();
    }
  }
}
