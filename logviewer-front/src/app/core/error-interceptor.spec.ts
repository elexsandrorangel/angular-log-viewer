import { ErrorInterceptor } from './error-interceptor';
import {AlertService} from '../services/alert.service';

describe('ErrorInterceptor', () => {
  let alertService: AlertService;

  it('should create an instance', () => {
    alertService = new AlertService();
    expect(new ErrorInterceptor(alertService)).toBeTruthy();
  });
});
