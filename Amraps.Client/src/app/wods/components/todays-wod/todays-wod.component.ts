import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {catchError} from 'rxjs/operators';
import {of} from 'rxjs/internal/observable/of';
import {WodsVm} from '../../../models/view.models';
import {WodsClient} from '../../clients/wods-client';


@Component({
  selector: 'todays-wod',
  templateUrl: './todays-wod.component.html'
})
export class TodaysWodComponent implements OnInit {
  public error = false;
  public loading = false;
  public wod: WodsVm;

  constructor(private route: ActivatedRoute, private _wodsClient: WodsClient) {
  }

  public ngOnInit(): void {
    this.loading = true;
    this._wodsClient.getTodaysWod().pipe(catchError(err => {
      console.log(err);
      return of(null);
    })).subscribe((data: WodsVm) => {
      this.wod = data ? data : null;
      this.loading = false;
    });
  }
}
