import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BjSimulatorComponent } from './bj-simulator.component';

describe('BjSimulatorComponent', () => {
  let component: BjSimulatorComponent;
  let fixture: ComponentFixture<BjSimulatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BjSimulatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BjSimulatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
