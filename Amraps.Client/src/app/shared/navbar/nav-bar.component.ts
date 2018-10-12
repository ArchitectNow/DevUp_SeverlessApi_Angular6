import {Component, ElementRef, Renderer2} from '@angular/core';
import {Location} from '@angular/common';
import {AuthService} from '../../auth/services/auth.service';

@Component({
    selector: 'app-navbar',
    templateUrl: './nav-bar.component.html'
})
export class NavBarComponent {
    constructor(private _authService: AuthService) {

    }

    public signOut() {
        this._authService.signOut(false);
    }
}
