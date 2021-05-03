import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { User } from '../../_models/user';
import { AppSettings } from '../../_helpers/appSettings';

// this allows the service to be injected into other components or services
@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  getUserAddresses(): Observable<any[]> {
    return this.http.get<any[]>(AppSettings.baseUrl + 'User/GetUserAddresses');
  }

  GetUserManufacturingProcessForDropDown(): Observable<any[]> {
    return this.http.post<any[]>(
      AppSettings.baseUrl + 'User/GetUserManufacturingProcessForDropDown',
      {}
    );
  }

  AddAddress(model: any): Observable<any> {
    return this.http.post(AppSettings.baseUrl + 'User/AddAddress', model);
  }

  GetLoggedInUserInformation(): Observable<any> {
    return this.http.get<any>(AppSettings.baseUrl + 'User/GetLoggedInUserInfo');
  }
}
