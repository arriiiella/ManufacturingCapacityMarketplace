import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettings } from 'src/app/_helpers/appSettings';
import { DataTablesResponse } from 'src/app/_models/dataTablesResponse';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  constructor(private http: HttpClient) {}

  GetOrderConfirmationDetails(model: any): Observable<any> {
    return this.http.post<any>(
      AppSettings.baseUrl + 'Order/GetOrderConfirmationDetails',
      model
    );
  }

  GetOrderByOrderNo(model: any): Observable<any> {
    return this.http.post<any>(
      AppSettings.baseUrl + 'Order/GetOrderByOrderNo',
      model
    );
  }

  CreateOrder(model: any): Observable<any> {
    return this.http.post<any>(
      AppSettings.baseUrl + 'Order/CreateOrder',
      model
    );
  }

  GetCustomerOrders(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Order/GetCustomerOrders',
      dtParams
    );
  }

  GetManufacturerOrders(dtParams: any): Observable<DataTablesResponse> {
    return this.http.post<DataTablesResponse>(
      AppSettings.baseUrl + 'Order/GetManufacturerOrders',
      dtParams
    );
  }
}
