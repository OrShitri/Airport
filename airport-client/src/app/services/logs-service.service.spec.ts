import { TestBed } from '@angular/core/testing';

import { LogsServiceService } from './logs-service.service';

describe('LogsServiceService', () => {
  let service: LogsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LogsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
