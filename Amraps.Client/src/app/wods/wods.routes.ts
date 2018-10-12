import {Routes} from '@angular/router';
import {TodaysWodComponent} from './components/todays-wod/todays-wod.component';
import {WodListComponent} from './components/wod-list/wod-list.component';
import {AuthorizedGuard} from '../auth/guards/authorized.guard';
import {WodDetailComponent} from './components/wod-detail/wod-detail.component';


export const WodRoutes: Routes = [
  {path: '', redirectTo: 'todays-wod', pathMatch: 'full'},
  {
    path: 'todays-wod',
    component: TodaysWodComponent,
    canActivate: [AuthorizedGuard],
    data: {
      authorize: ['wods.readonly', 'wods']
    }
  }, {
    path: 'list',
    component: WodListComponent,
    canActivate: [AuthorizedGuard],
    data: {
      authorize: ['wods.readonly', 'wods']
    }
  }, {
    path: 'details/:id',
    component: WodDetailComponent,
    canActivate: [AuthorizedGuard],
    data: {
      authorize: ['wods', 'wods.readonly']
    }
  }
];
