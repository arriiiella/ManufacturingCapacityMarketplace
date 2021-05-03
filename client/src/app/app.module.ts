import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/AuthGuard';
import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { ManufacturerLocationComponent } from './location/manufacturer-location.component';
import { TokenInterceptor } from './_helpers/header';
import { DataTablesModule } from 'angular-datatables';
import { MachineComponent } from './machine/machine.component';
import { CapacityComponent } from './capacity/capacity.component';
import { CustomNgbDateParserFormatter } from './_helpers/customNgbDateParserFormatter';
import { SearchComponent } from './search/search.component';
import { OrderConfirmationComponent } from './orders/order-confirmation/order-confirmation.component';
import { CustomerOrdersComponent } from './orders/customer-orders/customer-orders.component';
import { ManufacturerOrdersComponent } from './orders/manufacturer-orders/manufacturer-orders.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AccountComponent } from './account/account.component';
import { FooterComponent } from './footer/footer.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { LandingComponent } from './landing/landing.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    LoginComponent,
    ManufacturerLocationComponent,
    MachineComponent,
    CapacityComponent,
    SearchComponent,
    OrderConfirmationComponent,
    CustomerOrdersComponent,
    ManufacturerOrdersComponent,
    AccountComponent,
    FooterComponent,
    LandingComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgbModule,
    NgxUiLoaderModule,
    DataTablesModule,
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    ToastrModule.forRoot(),
    TabsModule.forRoot(),
    CarouselModule.forRoot(),
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    {
      provide: NgbDateParserFormatter,
      useFactory: () => new CustomNgbDateParserFormatter('longDate'),
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
