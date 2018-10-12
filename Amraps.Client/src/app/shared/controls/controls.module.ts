import {NgModule} from '@angular/core';
import {InputControlComponent} from './input-control/input-control.component';
import {MatInputModule} from '@angular/material';
import {CommonModule} from '@angular/common';

@NgModule({
    imports: [
      CommonModule,
      MatInputModule
    ],
    exports: [InputControlComponent],
    declarations: [InputControlComponent],
    providers: []
})
export class ControlsModule {
}
