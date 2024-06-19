import { TestBed } from '@angular/core/testing';

import { ExpenseManagerService } from './expense-manager.service';

describe('ExpenseManagerService', () => {
  let service: ExpenseManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpenseManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
