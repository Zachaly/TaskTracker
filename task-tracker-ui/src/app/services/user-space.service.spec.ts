import { TestBed } from '@angular/core/testing';

import { UserSpaceService } from './user-space.service';

describe('UserSpaceService', () => {
  let service: UserSpaceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserSpaceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
