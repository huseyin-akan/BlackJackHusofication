import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BjGameAreaComponent } from './bj-game-area.component';

describe('BjGameAreaComponent', () => {
  let component: BjGameAreaComponent;
  let fixture: ComponentFixture<BjGameAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BjGameAreaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BjGameAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
