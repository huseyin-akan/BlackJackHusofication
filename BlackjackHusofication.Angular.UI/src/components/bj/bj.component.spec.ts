import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BjComponent } from './bj.component';

describe('BjComponent', () => {
  let component: BjComponent;
  let fixture: ComponentFixture<BjComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BjComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
