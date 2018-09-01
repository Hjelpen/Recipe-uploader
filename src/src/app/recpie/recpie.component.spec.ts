import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecpieComponent } from './recpie.component';

describe('RecpieComponent', () => {
  let component: RecpieComponent;
  let fixture: ComponentFixture<RecpieComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecpieComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecpieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
