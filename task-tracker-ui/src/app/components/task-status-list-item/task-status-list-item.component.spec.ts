import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskStatusListItemComponent } from './task-status-list-item.component';

describe('TaskStatusListItemComponent', () => {
  let component: TaskStatusListItemComponent;
  let fixture: ComponentFixture<TaskStatusListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskStatusListItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskStatusListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
