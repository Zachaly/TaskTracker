import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTaskListItemComponent } from './user-task-list-item.component';

describe('UserTaskListItemComponent', () => {
  let component: UserTaskListItemComponent;
  let fixture: ComponentFixture<UserTaskListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserTaskListItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserTaskListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
