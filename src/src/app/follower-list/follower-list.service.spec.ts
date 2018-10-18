import { TestBed, inject } from '@angular/core/testing';

import { FollowerListService } from './follower-list.service';

describe('FollowerListService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FollowerListService]
    });
  });

  it('should be created', inject([FollowerListService], (service: FollowerListService) => {
    expect(service).toBeTruthy();
  }));
});
