import { Component, OnInit } from '@angular/core';
import {WodsClient} from '../../clients/wods-client';
import {WodsVm} from '../../../models/view.models';

@Component({
  selector: 'wod-list',
  templateUrl: './wod-list.component.html'
})
export class WodListComponent implements OnInit {
  public wods: WodsVm[] = [];

  constructor(private _wodClient: WodsClient) { }

  ngOnInit() {
    this._wodClient.getWods().subscribe((data: WodsVm[]) => {
      this.wods = data;
    });
  }

}
