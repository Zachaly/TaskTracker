import { ComponentFixture, TestBed } from '@angular/core/testing';

import ManagePermissionsPageComponent from './manage-permissions-page.component';

describe('ManagePermissionsPageComponent', () => {
  let component: ManagePermissionsPageComponent;
  let fixture: ComponentFixture<ManagePermissionsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManagePermissionsPageComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ManagePermissionsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
