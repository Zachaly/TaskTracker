import { TestBed } from '@angular/core/testing';

import { TaskStatusGroupService } from './task-status-group.service';

describe('TaskStatusGroupService', () => {
  let service: TaskStatusGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskStatusGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
