import {Injectable, Injector} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from './services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authService = this.injector.get(AuthService);
    const token = authService.getBearerToken();
    if (!token)
      return next.handle(req);

    const authHeader = `Bearer ${token}`;
    const authReq = req.clone({
      setHeaders: {Authorization: authHeader},
      reportProgress: true
    });

    return next.handle(authReq);
  }
}

export const AuthInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: AuthInterceptor,
  multi: true
};
