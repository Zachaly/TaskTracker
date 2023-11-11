import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListPageComponent } from './add-list-page.component';

describe('AddListPageComponent', () => {
  let component: AddListPageComponent;
  let fixture: ComponentFixture<AddListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
