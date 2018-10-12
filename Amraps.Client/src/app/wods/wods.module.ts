import {NgModule} from '@angular/core';
import {TodaysWodComponent} from './components/todays-wod/todays-wod.component';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {WodRoutes} from './wods.routes';
import {WodsClient} from './clients/wods-client';
import { WodListComponent } from './components/wod-list/wod-list.component';
import {AuthModule} from '../auth/auth.module';
import { WodDetailComponent } from './components/wod-detail/wod-detail.component';
import { WodDisplayComponent } from './components/wod-display/wod-display.component';
import { EditWodComponent } from './components/edit-wod/edit-wod.component';


@NgModule({
    declarations: [TodaysWodComponent, WodListComponent, WodDetailComponent, WodDisplayComponent, EditWodComponent],
    imports: [
      AuthModule,
      CommonModule,
      RouterModule.forChild(WodRoutes)],
    exports: [],
    providers: [WodsClient]
})
export class WodsModule {

}
