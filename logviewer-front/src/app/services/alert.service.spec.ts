import { TestBed } from '@angular/core/testing';

import { AlertService } from './alert.service';
import {HttpClientTestingModule } from '@angular/common/http/testing';

describe('AlertService', () => {
  let service: AlertService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule ]
    });
    service = TestBed.inject(AlertService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
