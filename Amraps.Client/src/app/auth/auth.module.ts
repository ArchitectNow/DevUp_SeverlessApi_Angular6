import {NgModule} from '@angular/core';
import {AuthCallbackComponent} from './components/auth-callback.component';
import {AuthService} from './services/auth.service';
import {AuthInterceptorProvider} from './auth.interceptor';
import {AuthenticationGuard} from './guards/authentication.guard';
import {OAuthModule} from 'angular-oauth2-oidc';
import {HttpClientModule} from '@angular/common/http';
import {SessionService} from '../core/services/session.service';
import {AuthLandingComponent} from './components/auth-landing.component';
import {AuthorizedGuard} from './guards/authorized.guard';
import {AuthorizedDirective} from './directives/authorized.directive';
import {LogoutConfirmComponent} from './components/logout-confirm.component';

@NgModule({
  imports: [
    HttpClientModule,
    OAuthModule.forRoot()
  ],
  exports: [AuthorizedDirective],
  declarations: [
    AuthCallbackComponent,
    AuthLandingComponent,
    AuthorizedDirective,
    LogoutConfirmComponent
  ],
  providers: [
    AuthService,
    AuthenticationGuard,
    AuthorizedGuard,
    AuthInterceptorProvider,
    SessionService]
})
export class AuthModule {

}
