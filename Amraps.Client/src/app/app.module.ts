import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import {RouterModule} from '@angular/router';
import {AppRoutes} from './app.routes';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {LayoutsModule} from './layouts/layouts.module';
import {environment} from '../environments/environment';
import {AuthorityUrl} from './auth/services/auth.service';
import {AuthModule} from './auth/auth.module';

import {DevToolsExtension, NgRedux} from '@angular-redux/store';
import {AppState, IAppState} from './store/app.state';
import {provideReduxForms} from '@angular-redux/form';
import {rootReducer} from './store/reducers/root.reducer';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ApiBaseUrl} from './services/api-client-base';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    LayoutsModule,
    BrowserModule,
    AuthModule,
    BrowserAnimationsModule,
    OAuthModule.forRoot(),
    RouterModule.forRoot(AppRoutes)
  ],
  providers: [
    {provide: ApiBaseUrl, useValue: environment.appConfig.apiBaseUrl},
    {provide: AuthorityUrl, useValue: environment.appConfig.authenticationAuthority}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  // constructor(store: NgRedux<IAppState>,
  //             devTools: DevToolsExtension) {
  //
  //   store.configureStore(
  //     rootReducer,
  //     AppState,
  //     null,
  //     devTools.isEnabled() ? [devTools.enhancer()] : []);
  //
  //   // Enable syncing of Angular router state with our Redux store.
  //   // if (ngReduxRouter) {
  //   //     ngReduxRouter.initialize();
  //   // }
  //
  //   // Enable syncing of Angular form state with our Redux store.
  //   provideReduxForms(store);
  // }
}
