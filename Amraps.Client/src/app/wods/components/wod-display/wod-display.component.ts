import {Component, Input, OnInit} from '@angular/core';
import {WodsVm} from '../../../models/view.models';

@Component({
  selector: 'wod-display',
  templateUrl: './wod-display.component.html',
  styleUrls: ['./wod-display.component.scss']
})
export class WodDisplayComponent implements OnInit {
  @Input() wod: WodsVm;

  constructor() { }

  ngOnInit() {
  }

}
