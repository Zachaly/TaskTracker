import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskStatusPageComponent } from './task-status-page.component';

describe('TaskStatusPageComponent', () => {
  let component: TaskStatusPageComponent;
  let fixture: ComponentFixture<TaskStatusPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskStatusPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskStatusPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
