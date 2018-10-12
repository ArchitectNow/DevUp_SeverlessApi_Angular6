import {Component, ElementRef, OnInit} from '@angular/core';
import {AuthService} from '../../auth/services/auth.service';
import {NavigationPaths} from '../navigation.paths';
import {isNullOrUndefined} from 'util';
import {INavigationPaths} from '../INavigationPaths';

@Component({
    selector: 'app-sidebar',
    templateUrl: './side-bar.component.html'
})
export class SideBarComponent implements OnInit {
    public menuItems: INavigationPaths[];
    public displayName: string;

    constructor(private elemRef: ElementRef, private _authService: AuthService) { }

    isNotMobileMenu() {
        const w = window.innerWidth;
        if (w > 991) {
            return false;
        }
        return true;
    }

    public ngOnInit() {
        this.displayName = this._authService.displayName;
        const isWindows = navigator.platform.indexOf('Win') > -1 ? true : false;
        if (isWindows) {
            // // if we are on windows OS we activate the perfectScrollbar function
            // const sidebar = document.querySelectorAll('.sidebar-wrapper');
            // $sidebar.perfectScrollbar();
            // // if we are on windows OS we activate the perfectScrollbar function
            // jQuery('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();
            // jQuery('html').addClass('perfect-scrollbar-on');
        } else {
            const elem = this.elemRef.nativeElement.querySelector('html');
            if (elem)
                elem.addClass('perfect-scrollbar-off');
        }
        this.menuItems = NavigationPaths.filter(i => isNullOrUndefined(i.visible) || i.visible === true);
    }

  public confirmSignOut() {
      this._authService.signOut(false);
  }

  public visibleItems(children: INavigationPaths[]): INavigationPaths[] {
    return children.filter(c => isNullOrUndefined(c.visible) || c.visible === true);
  }
}



