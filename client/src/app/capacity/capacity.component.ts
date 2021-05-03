import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  ModalDismissReasons,
  NgbDateParserFormatter,
  NgbDateStruct,
  NgbModal,
  NgbTimeStruct,
} from '@ng-bootstrap/ng-bootstrap';
import { DataTableDirective } from 'angular-datatables';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subject, Subscription } from 'rxjs';
import { SearchCriteria } from '../_models/data-table';
import * as Capacity from '../_models/capacity';
import { CapacityService } from '../_services/capacity/capacity.service';
import { UserService } from '../_services/user/user.service';

@Component({
  selector: 'app-capacity',
  templateUrl: './capacity.component.html',
  styleUrls: ['./capacity.component.css'],
})
export class CapacityComponent implements OnInit {
  MachineCapacityEntryModel: Capacity.MachineCapacityEntry = new Capacity.MachineCapacityEntry();
  machineCapacityEntryDate: NgbDateStruct;
  machineCapacityEntryTime: NgbTimeStruct;
  searchCriteria: SearchCriteria = { isPageLoad: false, filter: '' };
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  SpareCapacitiesModel: Capacity.SpareCapacities[] = [];
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  timerSubscription: Subscription;
  closeResult: string;
  locationId = 0;
  machineId = 0;
  capacityViewVM: any;

  constructor(
    private ngbDateParserFormatter: NgbDateParserFormatter,
    private _Activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private capacityService: CapacityService,
    private ngxService: NgxUiLoaderService
  ) {
    this._Activatedroute.paramMap.subscribe((params) => {
      this.machineId = Number(params.get('mid'));
      this.locationId = Number(params.get('lid'));
    });
    this.loadPageDetails();
    this.machineCapacityEntryDate = this.setDefaultDate();
    this.MachineCapacityEntryModel.StartTime = {
      hour: 0,
      minute: 0,
      second: 0,
    };
  }

  loadPageDetails() {
    this.ngxService.start();
    var model = {
      LocationId: this.locationId,
      MachineId: this.machineId,
    };
    this.capacityService.GetCapacityPageInfo(model).subscribe(
      (response) => {
        this.capacityViewVM = response;
        console.log(response);
        this.ngxService.stop();
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

  addMachineEntry() {
    if (!this.MachineCapacityEntryModel.Capacity) {
      this.toastr.error('Please enter capacity!');
    } else if (this.machineCapacityEntryTime == undefined) {
      this.toastr.error('Please select valid start time!');
    } else {
      this.ngxService.start();
      this.MachineCapacityEntryModel.MachineId = this.machineId;
      this.MachineCapacityEntryModel.Date =
        this.machineCapacityEntryDate.month +
        '/' +
        this.machineCapacityEntryDate.day +
        '/' +
        this.machineCapacityEntryDate.year;
      this.MachineCapacityEntryModel.StartTime =
        this.machineCapacityEntryTime.hour +
        ':' +
        this.machineCapacityEntryTime.minute;
      this.capacityService
        .AddMachineCapacityEntry(this.MachineCapacityEntryModel)
        .subscribe(
          (response) => {
            this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
              dtInstance.ajax.reload();
            });
            this.ngxService.stop();
            this.toastr.success('Entry is successfully added');
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
      this.MachineCapacityEntryModel = new Capacity.MachineCapacityEntry();
    }
  }

  onSelectDate(date: NgbDateStruct) {
    if (date != null) {
      this.machineCapacityEntryDate = date;
      this.MachineCapacityEntryModel.Date = this.ngbDateParserFormatter.format(
        date
      );
    }
  }

  deletePopup(content, machineId) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
          if (result === 'yes') {
            this.deleteCapacity(machineId);
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

  setDefaultDate(): NgbDateStruct {
    var startDate = new Date();
    let startYear = startDate.getFullYear().toString();
    let startMonth = startDate.getMonth() + 1;
    let startDay = startDate.getDate();

    return this.ngbDateParserFormatter.parse(
      startYear + '-' + startMonth.toString() + '-' + startDay
    );
  }

  ngOnInit(): void {
    this.loadCapacityTable();
  }

  loadCapacityTable() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      ajax: (dataTablesParameters: any, callback) => {
        this.searchCriteria.filter = this.locationId + '|' + this.machineId;
        dataTablesParameters.searchCriteria = this.searchCriteria;
        this.capacityService
          .GetSpareCapacityView(dataTablesParameters)
          .subscribe((resp) => {
            that.SpareCapacitiesModel = resp.data;
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

  deleteCapacity(entryNo) {
    this.ngxService.start();
    console.log(entryNo);
    let model = {
      EntryNo: entryNo,
    };
    this.capacityService.DeleteCapacityEntry(model).subscribe(
      (response) => {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
          dtInstance.ajax.reload();
        });
        this.ngxService.stop();
        this.toastr.success('Capacity is successfully deleted');
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
    this.MachineCapacityEntryModel = new Capacity.MachineCapacityEntry();
    this.modalService.dismissAll();
  }
}
