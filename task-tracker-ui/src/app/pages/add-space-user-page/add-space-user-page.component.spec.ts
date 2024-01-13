import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSpaceUserPageComponent } from './add-space-user-page.component';

describe('AddSpaceUserPageComponent', () => {
  let component: AddSpaceUserPageComponent;
  let fixture: ComponentFixture<AddSpaceUserPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddSpaceUserPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddSpaceUserPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
