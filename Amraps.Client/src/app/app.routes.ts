import {Routes} from '@angular/router';
import {AdminLayoutComponent} from './layouts/admin.layout.component';
import {PageNotFoundComponent} from './pages/page-not-found.component';
import {AuthenticationGuard} from './auth/guards/authentication.guard';
import {AuthCallbackComponent} from './auth/components/auth-callback.component';
import {AuthLandingComponent} from './auth/components/auth-landing.component';
import {AuthorizedGuard} from './auth/guards/authorized.guard';
import {LogoutConfirmComponent} from './auth/components/logout-confirm.component';

export const AppRoutes: Routes = [
  {path: '', redirectTo: '/app/wods/todays-wod', pathMatch: 'full'},
  {
    path: '',
    children: [{
      path: 'authenticating',
      component: AuthLandingComponent
    }, {
      path: 'signin-oidc',
      component: AuthCallbackComponent
    }],
  }, {
    path: 'logout-confirm',
    canActivate: [AuthenticationGuard],
    component: LogoutConfirmComponent
  },
  {
    path: 'app',
    component: AdminLayoutComponent,
    canActivateChild: [AuthenticationGuard, AuthorizedGuard],
    children: [{
      path: 'wods',
      loadChildren: './wods/wods.module#WodsModule'
    }, {
      path: 'financial',
      loadChildren: './financial/financial.module#FinancialModule'
    }]
  }, {
    path: '**', component: PageNotFoundComponent
  }
];

export const appRoutingProviders: any[] = [];
