import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettings } from 'src/app/_helpers/appSettings';

@Injectable({
  providedIn: 'root',
})
export class LookUpService {
  constructor(private http: HttpClient) {}

  getIndustries(): Observable<any[]> {
    return this.http.get<any[]>(AppSettings.baseUrl + 'LookUp/GetIndustries');
  }
  getCountries(): Observable<any[]> {
    return this.http.get<any[]>(AppSettings.baseUrl + 'LookUp/GetCountries');
  }
}
