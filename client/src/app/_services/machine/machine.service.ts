import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettings } from 'src/app/_helpers/appSettings';
import { DataTablesResponse } from 'src/app/_models/dataTablesResponse';

@Injectable({
  providedIn: 'root',
})
export class MachineService {
  constructor(private http: HttpClient) {}

  AddMachine(model: any): Observable<any> {
    return this.http.post(AppSettings.baseUrl + 'Machine/AddMachine', model);
  }

  GetUserMachines(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Machine/GetUserMachines',
      dtParams
    );
  }

  DeleteUserMachine(model: any): Observable<any> {
    return this.http.post(
      AppSettings.baseUrl + 'Machine/DeleteUserMachine',
      model
    );
  }

  UpdateUserMachine(model: any): Observable<any> {
    return this.http.put(
      AppSettings.baseUrl + 'Machine/UpdateUserMachine',
      model
    );
  }
}
