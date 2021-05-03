import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './_guards/AuthGuard';
import { SearchComponent } from './search/search.component';
import { CapacityComponent } from './capacity/capacity.component';
import { CustomerOrdersComponent } from './orders/customer-orders/customer-orders.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MachineComponent } from './machine/machine.component';
import { ManufacturerOrdersComponent } from './orders/manufacturer-orders/manufacturer-orders.component';
import { ManufacturerLocationComponent } from './location/manufacturer-location.component';
import { OrderConfirmationComponent } from './orders/order-confirmation/order-confirmation.component';
import { RegisterComponent } from './register/register.component';
import { AccountComponent } from './account/account.component';
import { LandingComponent } from './landing/landing.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'landing', component: LandingComponent },
      {
        path: 'locations',
        component: ManufacturerLocationComponent,
      },
      {
        path: 'machines',
        component: MachineComponent,
      },
      {
        path: 'machines/:id',
        component: MachineComponent,
      },
      {
        path: 'capacity/:mid/:lid',
        component: CapacityComponent,
      },
      {
        path: 'search',
        component: SearchComponent,
      },
      {
        path: 'order-confirmation',
        component: OrderConfirmationComponent,
      },
      {
        path: 'customer-orders',
        component: CustomerOrdersComponent,
      },
      {
        path: 'manufacturer-orders',
        component: ManufacturerOrdersComponent,
      },
      {
        path: 'account',
        component: AccountComponent,
      },
      { path: '**', component: HomeComponent, pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
