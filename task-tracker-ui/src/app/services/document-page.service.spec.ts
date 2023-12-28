import { TestBed } from '@angular/core/testing';

import { DocumentPageService } from './document-page.service';

describe('DocumentPageService', () => {
  let service: DocumentPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DocumentPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
