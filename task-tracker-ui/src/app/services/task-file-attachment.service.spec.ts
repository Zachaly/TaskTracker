import { TestBed } from '@angular/core/testing';

import { TaskFileAttachmentService } from './task-file-attachment.service';

describe('TaskFileAttachmentService', () => {
  let service: TaskFileAttachmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskFileAttachmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
