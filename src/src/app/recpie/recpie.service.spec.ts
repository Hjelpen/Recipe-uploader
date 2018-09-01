import { TestBed, inject } from '@angular/core/testing';

import { RecpieService } from './recpie.service';

describe('RecpieService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RecpieService]
    });
  });

  it('should be created', inject([RecpieService], (service: RecpieService) => {
    expect(service).toBeTruthy();
  }));
});
