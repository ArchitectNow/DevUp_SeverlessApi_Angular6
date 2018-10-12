import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../services/auth.service';
import {Observable} from 'rxjs';

@Component({
    selector: 'auth-callback',
    templateUrl: './auth-callback.component.html'
})
export class AuthCallbackComponent implements OnInit {

    constructor(private router: Router,
                private _authService: AuthService) {
    }

    ngOnInit(): void {
      this._authService.onAuthenticated.subscribe(() => {
        console.log('Auth Callback authorized');
        this.router.navigateByUrl('app/wods/todays-wod');
      });
    }
}
