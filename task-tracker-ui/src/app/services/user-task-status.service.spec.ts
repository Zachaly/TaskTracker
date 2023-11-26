import { TestBed } from '@angular/core/testing';

import { UserTaskStatusService } from './user-task-status.service';

describe('UserTaskStatusService', () => {
  let service: UserTaskStatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserTaskStatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
