import {Component, OnInit} from '@angular/core';
import {AuthService} from '../services/auth.service';

@Component({
    selector: 'logout-confirm',
    templateUrl: './logout-confirm.component.html',
  styleUrls: ['./logout.scss']
})
export class LogoutConfirmComponent implements OnInit {

    constructor(private _authService: AuthService) {

    }

    ngOnInit(): void {

    }

    public signOut() {
      this._authService.signOut(true);
    }

    public goBack() {
      window.history.back();
    }
}
