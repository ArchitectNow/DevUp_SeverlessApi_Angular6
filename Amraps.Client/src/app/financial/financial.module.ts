import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinancialDashboardComponent } from './components/financial-dashboard/financial-dashboard.component';
import {FinancialClient} from './clients/financial-client';
import {RouterModule} from '@angular/router';
import {FinancialRoutes} from './financial.routes';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(FinancialRoutes)
  ],
  declarations: [FinancialDashboardComponent],
  exports: [FinancialDashboardComponent],
  providers: [FinancialClient]
})
export class FinancialModule { }
