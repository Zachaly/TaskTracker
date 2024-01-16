import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTaskAssignedUsersDialogComponent } from './manage-task-assigned-users-dialog.component';

describe('ManageTaskAssignedUsersDialogComponent', () => {
  let component: ManageTaskAssignedUsersDialogComponent;
  let fixture: ComponentFixture<ManageTaskAssignedUsersDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageTaskAssignedUsersDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageTaskAssignedUsersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
