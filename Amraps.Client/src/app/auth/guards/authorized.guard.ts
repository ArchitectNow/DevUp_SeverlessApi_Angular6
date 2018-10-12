import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, CanActivateChild, RouterStateSnapshot} from '@angular/router';
import {Observable, of} from 'rxjs';
import {AuthService} from '../services/auth.service';


@Injectable()
export class AuthorizedGuard implements CanActivate, CanActivateChild {

  constructor(
    private _authService: AuthService) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this._canActivate(route);
  }

  public canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this._canActivate(route);
  }

  private _canActivate(route: ActivatedRouteSnapshot) {
    if (!route.data || !route.data['authorize']) {
      return of(true);
    }
    const reqRoles = route.data['authorize'] as string[];
    return of(this._authService.hasRoles(reqRoles));
  }
}
