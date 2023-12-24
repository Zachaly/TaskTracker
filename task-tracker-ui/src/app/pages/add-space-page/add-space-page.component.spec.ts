import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSpacePageComponent } from './add-space-page.component';

describe('AddSpacePageComponent', () => {
  let component: AddSpacePageComponent;
  let fixture: ComponentFixture<AddSpacePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddSpacePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddSpacePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
