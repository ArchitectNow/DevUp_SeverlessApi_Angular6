import {Injectable, OnDestroy} from '@angular/core';
import {Observable, Subscription, Subject, fromEvent, interval} from 'rxjs';
import {environment} from '../../../environments/environment';
import {debounceTime} from 'rxjs/operators';


@Injectable()
export class SessionService implements OnDestroy {
  private _sessionTimeOut = environment.sessionTimeout; // minutes
  private _sessionCheck = 30000; // milliseconds
  private _scrollSub: Subscription;
  private _keyPressSub: Subscription;
  private _clickSub: Subscription;
  private _lastInteraction = new Date();
  private _sessionHandler: Subscription = null;

  private $sessionTimedOut = new Subject();
  public onSessionTimedOut = this.$sessionTimedOut.asObservable();

  private sessionTimeoutObservable: Observable<any>;

  constructor() {
  }

  private get hasTimedOut(): boolean {
    const timeOut = new Date(this._lastInteraction);
    timeOut.setMinutes(this._lastInteraction.getMinutes() + this._sessionTimeOut);
    const now = new Date();
    return timeOut < now;
  }

  ngOnDestroy(): void {
    this._scrollSub.unsubscribe();
    this._clickSub.unsubscribe();
    this._keyPressSub.unsubscribe();
    this.$sessionTimedOut.unsubscribe();
  }

  public startSessionTimeout() {
    if (this._sessionHandler) {
      console.log('Starting already started.');
      return;
    }
    this.initialize();
    console.log('Starting session');
    this._sessionHandler = this.sessionTimeoutObservable.subscribe(() => {
      console.log('Session check');
      if (this.hasTimedOut) {
        console.log('Session expired');
        // Raise event to anyone listening.
        this.$sessionTimedOut.next();
      }
    });
  }

  private initialize() {
    console.log(`Initializing session timeout: ${this._sessionTimeOut}min, session check interval: ${this._sessionCheck / 1000}sec.`);
    this.sessionTimeoutObservable = interval(this._sessionCheck);
    this._scrollSub = fromEvent(document, 'scroll')
      .pipe(debounceTime(500))
      .subscribe(() => {
        console.log('reset session: scroll');
        this.setInteractionDate();
      });
    this._keyPressSub = fromEvent(document, 'keypress').subscribe(() => {
      console.log('reset session: keypress');
      this.setInteractionDate();
    });
    this._clickSub = fromEvent(document, 'click').subscribe(() => {
      console.log('reset session: clicked');
      this.setInteractionDate();
    });
  }

  private setInteractionDate(): void {
    this._lastInteraction = new Date();
  }
}
