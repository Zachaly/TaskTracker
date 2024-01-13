import { TestBed } from '@angular/core/testing';

import { SpaceUserService } from './space-user.service';

describe('SpaceUserService', () => {
  let service: SpaceUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SpaceUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
