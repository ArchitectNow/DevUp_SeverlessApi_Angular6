import { Observable} from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {ApiClientBase} from '../../services/api-client-base';
import {WodsVm} from '../../models/view.models';

export const ApiBaseUrl = new InjectionToken<string>('ApiBaseUrl');

@Injectable()
export class WodsClient extends ApiClientBase<WodsVm> {

  constructor(http: HttpClient, @Optional() @Inject(ApiBaseUrl) baseUrl?: string) {
   super(http, baseUrl);
  }

  public getTodaysWod(): Observable<WodsVm> {
    return this.get('/wods/today', WodsVm.fromJS);
  }

  public getWods(): Observable<WodsVm[]> {
    return this.getMany('/wods', WodsVm.fromJS);
  }

  public getWod(id: string): Observable<WodsVm> {
    return this.get('/wods', WodsVm.fromJS, id);
  }


  public deleteWod(id: string) {
    return this.delete('/wods', id);
  }
}

