import {TestBed} from '@angular/core/testing';

import {LogsService} from './logs.service';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';
import {Log} from '../models/log';

describe('LogsService', () => {
  let service: LogsService;
  let httpMock: HttpTestingController;

  const fakeDataResponse: Log[] = [];
  for (let i = 0; i < 4; i++){
    const log = new Log();
    log.ip = '1.1.1.1';
    log.remoteUser = '';
    log.timeLocal = '2021-01-01T00:01:00';
    log.bytesSent = '200';
    log.requestUrl = '/health-check';
    log.statusCode = '200';
    log.httpUserAgent = 'bot';

    fakeDataResponse.push(log);
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(LogsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAll() should return data', () => {
    service.getAll().subscribe((res) => {
      expect(res).toEqual(fakeDataResponse);
    });

    const req = httpMock.expectOne('https://localhost:44309/api/logs');
    expect(req.request.method).toBe('GET');
    req.flush(fakeDataResponse);
  });

  afterEach(() => {
    httpMock.verify();
  });
});
