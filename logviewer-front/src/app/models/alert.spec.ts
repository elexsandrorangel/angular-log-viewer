import {Alert, AlertType} from './alert';

describe('Alert', () => {
  it('should create an instance', () => {
    const id = '';
    const type: AlertType = AlertType.Success;
    const message = 'Test alert';
    const autoClose = true;
    const keepAfterRouteChange = false;
    const fade = false;
    expect(new Alert(id, type, message, autoClose, keepAfterRouteChange, fade)).toBeTruthy();
  });
});
