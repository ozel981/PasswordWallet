import { TestBed } from '@angular/core/testing';

import { SharedPasswordService } from './shared-password.service';

describe('SharedPasswordService', () => {
  let service: SharedPasswordService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SharedPasswordService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
