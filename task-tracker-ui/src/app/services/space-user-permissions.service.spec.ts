import { TestBed } from '@angular/core/testing';

import { SpaceUserPermissionsService } from './space-user-permissions.service';

describe('SpaceUserPermissionsService', () => {
  let service: SpaceUserPermissionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SpaceUserPermissionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
