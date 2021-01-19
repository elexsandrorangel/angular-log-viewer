import { Injectable } from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {Alert, AlertType} from '../models/alert';
import {filter} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private subject = new Subject<Alert>();
  private defaultId = 'default-alert';

  constructor() { }

  // enable subscribing to alerts observable
  onAlert(id = this.defaultId): Observable<Alert> {
    return this.subject.asObservable().pipe(filter(x => x && x.id === id));
  }

  // convenience methods
  success(message: string, options?: Partial<Alert>): void {
    this.alert(message, AlertType.Success, options);
  }

  error(message: string, options?: Partial<Alert>): void {
    this.alert(message, AlertType.Error, options);
  }

  info(message: string, options?: Partial<Alert>): void {
    this.alert(message, AlertType.Info, options);
  }

  warn(message: string, options?: Partial<Alert>): void {
    this.alert(message, AlertType.Warning, options);
  }

  // main alert method
  alert(message: string, type: AlertType, options: Partial<Alert> = {}): void {
    const id = options.id || this.defaultId;
    const alert = new Alert(id, type, message, options.autoClose, options.keepAfterRouteChange);
    this.subject.next(alert);
  }

  // clear alerts
  clear(id = this.defaultId): void {
    this.subject.next(new Alert(id));
  }
}
