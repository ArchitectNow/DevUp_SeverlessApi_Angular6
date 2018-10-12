import {Component, OnInit} from '@angular/core';
import {FinancialClient} from '../../clients/financial-client';
import {catchError} from 'rxjs/operators';
import {of} from 'rxjs/internal/observable/of';

@Component({
  selector: 'app-financial-dashboard',
  templateUrl: './financial-dashboard.component.html'
})
export class FinancialDashboardComponent implements OnInit {
  public error: any;
  public financials: string[] = [];

  constructor(private _financialClient: FinancialClient) {
  }

  ngOnInit() {
    this._financialClient.getDashboard()
      .pipe(catchError(err => {
        this.error = 'Error loading financial dashboard';
        return of(null);
      }))
      .subscribe((data: string[]) => {
        this.financials = data;
      });
  }

}
