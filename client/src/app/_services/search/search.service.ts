import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataTablesResponse } from 'src/app/_models/dataTablesResponse';
import { AppSettings } from 'src/app/_helpers/appSettings';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  constructor(private http: HttpClient) {}

  FilterSpareCapacity(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Search/FilterSpareCapacity',
      dtParams
    );
  }

  GetSearchViewCities(): Observable<any> {
    return this.http.get<any>(
      AppSettings.baseUrl + 'Search/GetSearchViewCities'
    );
  }
}
