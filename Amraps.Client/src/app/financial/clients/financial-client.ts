import {Inject, Injectable, Optional} from '@angular/core';
import {ApiClientBase} from '../../services/api-client-base';
import {HttpClient} from '@angular/common/http';
import {ApiBaseUrl} from '../../wods/clients/wods-client';
import {Observable} from 'rxjs/internal/Observable';
import {of} from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root'
})
export class FinancialClient extends ApiClientBase<any> {

  constructor(http: HttpClient, @Optional() @Inject(ApiBaseUrl) baseUrl?: string) {
    super(http, baseUrl);
  }

  getDashboard(): Observable<any> {
    return this.getMany('/financial', (s: string) => s);
  }
}

