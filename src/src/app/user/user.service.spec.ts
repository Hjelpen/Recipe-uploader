import { TestBed, inject } from '@angular/core/testing';
import { UserService1 } from './user.service';

describe('UserService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserService1]
    });
  });

  it('should be created', inject([UserService1], (service: UserService1) => {
    expect(service).toBeTruthy();
  }));
});
