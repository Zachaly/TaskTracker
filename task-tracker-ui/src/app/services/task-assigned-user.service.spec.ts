import { TestBed } from '@angular/core/testing';

import { TaskAssignedUserService } from './task-assigned-user.service';

describe('TaskAssignedUserService', () => {
  let service: TaskAssignedUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskAssignedUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
