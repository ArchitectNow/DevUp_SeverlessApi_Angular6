import {Component, OnInit} from '@angular/core';
import {AuthService} from './auth/services/auth.service';
import {NavigationStart, Router} from '@angular/router';
import {filter} from 'rxjs/operators';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  constructor(private _authService: AuthService, private router: Router) {

  }

  public ngOnInit(): void {
    this._authService.configureAuthentication();
    this.router.events.pipe(filter(evt => evt instanceof NavigationStart))
      .subscribe((evt: NavigationStart) => {
        console.log(evt);
      });
  }
}
