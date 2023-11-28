import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateStatusGroupPageComponent } from './update-status-group-page.component';

describe('UpdateStatusGroupPageComponent', () => {
  let component: UpdateStatusGroupPageComponent;
  let fixture: ComponentFixture<UpdateStatusGroupPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateStatusGroupPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateStatusGroupPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
