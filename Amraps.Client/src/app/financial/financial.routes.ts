import {Routes} from '@angular/router';
import {FinancialDashboardComponent} from './components/financial-dashboard/financial-dashboard.component';


export const FinancialRoutes: Routes = [
  {path: '', redirectTo: 'list', pathMatch: 'full'},
  {
    path: 'list',
    component: FinancialDashboardComponent
  }];
