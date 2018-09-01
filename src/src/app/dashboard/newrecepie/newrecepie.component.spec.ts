import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewrecepieComponent } from './newrecepie.component';

describe('NewrecepieComponent', () => {
  let component: NewrecepieComponent;
  let fixture: ComponentFixture<NewrecepieComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewrecepieComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewrecepieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
