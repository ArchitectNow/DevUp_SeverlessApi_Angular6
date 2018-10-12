import { Component, OnInit } from '@angular/core';
import {catchError, mergeMap, take} from 'rxjs/operators';
import {of} from 'rxjs/internal/observable/of';
import {WodsVm} from '../../../models/view.models';
import {WodsClient} from '../../clients/wods-client';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'wod-detail',
  templateUrl: './wod-detail.component.html'
})
export class WodDetailComponent implements OnInit {
  private _wodId: string;
  public error = false;
  public loading = false;
  public wod: WodsVm;

  constructor(private _wodsClient: WodsClient,
              private _route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.loading = true;

    this._route.params.pipe(
      take(1),
      mergeMap((params) => {
        if (params['id']) {
          this._wodId = params['id'];
          return this._wodsClient.getWod(this._wodId);
        } else {
          return of(null);
        }
    }), catchError(err => {
        console.log(err);
        return of(null);
      }))
      .subscribe((wod: WodsVm) => {
        this.wod = wod ? wod : null;
        this.loading = false;
    });
  }

}
