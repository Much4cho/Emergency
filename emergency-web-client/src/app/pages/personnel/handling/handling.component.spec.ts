import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HandlingComponent } from './handling.component';

describe('HandlingComponent', () => {
  let component: HandlingComponent;
  let fixture: ComponentFixture<HandlingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HandlingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HandlingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
