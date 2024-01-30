import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BjRoomsComponent } from './bj-rooms.component';

describe('BjRoomsComponent', () => {
  let component: BjRoomsComponent;
  let fixture: ComponentFixture<BjRoomsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BjRoomsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BjRoomsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
