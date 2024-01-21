import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayBjComponent } from './play-bj.component';

describe('PlayBjComponent', () => {
  let component: PlayBjComponent;
  let fixture: ComponentFixture<PlayBjComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlayBjComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PlayBjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
