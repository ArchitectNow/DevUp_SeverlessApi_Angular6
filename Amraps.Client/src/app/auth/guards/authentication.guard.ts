import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, CanActivateChild, RouterStateSnapshot} from '@angular/router';
import {OAuthService} from 'angular-oauth2-oidc';
import {Observable} from 'rxjs';

@Injectable()
export class AuthenticationGuard implements CanActivate, CanActivateChild {

  constructor(private oAuthService: OAuthService) {

  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this._hasValidToken();
  }

  public canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this._hasValidToken();
  }

  private _hasValidToken(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }
}
