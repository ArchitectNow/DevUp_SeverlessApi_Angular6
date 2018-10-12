import {Directive, Input, TemplateRef, ViewContainerRef} from '@angular/core';
import {AuthService} from '../services/auth.service';

@Directive({
  selector: '[authorize]',
})
export class AuthorizedDirective {
  constructor(private _authService: AuthService,
              private viewRef: ViewContainerRef,
              private templateRef: TemplateRef<any>) { }

  @Input()
  set authorize(roles: string[]) {
    if (!roles || !roles.length) {
      console.log(`Directive roles: true`, roles);
      this.viewRef.createEmbeddedView(this.templateRef);
      return;
    }
    const hasPermission = this._authService.hasRoles(roles);
    console.log(`Directive permission: ${hasPermission}`, roles);
    if (hasPermission) {
      this.viewRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewRef.clear();
    }
  }
}
