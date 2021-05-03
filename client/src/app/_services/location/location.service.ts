import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettings } from 'src/app/_helpers/appSettings';
import { DataTablesResponse } from 'src/app/_models/dataTablesResponse';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  constructor(private http: HttpClient) {}

  getUserManufacturingLocation(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Location/GetUserManufacturingLocation',
      dtParams
    );
  }

  getUserManufacturingLocationForDropDown(): Observable<any[]> {
    return this.http.post<any[]>(
      AppSettings.baseUrl + 'Location/GetUserManufacturingLocationForDropDown',
      {}
    );
  }

  AddUserLocation(model: any): Observable<any> {
    return this.http.post(
      AppSettings.baseUrl + 'Location/AddManufacturerLocation',
      model
    );
  }

  DeleteUserLocation(model: any): Observable<any> {
    return this.http.post(
      AppSettings.baseUrl + 'Location/DeleteUserManufacturingLocation',
      model
    );
  }
}
