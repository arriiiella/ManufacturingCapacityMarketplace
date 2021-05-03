import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettings } from 'src/app/_helpers/appSettings';
import { DataTablesResponse } from 'src/app/_models/dataTablesResponse';

@Injectable({
  providedIn: 'root',
})
export class CapacityService {
  constructor(private http: HttpClient) {}

  GetSpareCapacityView(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Capacity/GetSpareCapacityView',
      dtParams
    );
  }

  GetCapacityPageInfo(model: any): Observable<any> {
    return this.http.post<any>(
      AppSettings.baseUrl + 'Capacity/GetCapacityPageInfo',
      model
    );
  }

  AddMachineCapacityEntry(model: any): Observable<any> {
    return this.http.post<any>(
      AppSettings.baseUrl + 'Capacity/AddMachineCapacityEntry',
      model
    );
  }

  DeleteCapacityEntry(model: any): Observable<any> {
    return this.http.post(
      AppSettings.baseUrl + 'Capacity/DeleteCapacityEntry',
      model
    );
  }
}
