import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarListLinkComponent } from './side-bar-list-link.component';

describe('SideBarListLinkComponent', () => {
  let component: SideBarListLinkComponent;
  let fixture: ComponentFixture<SideBarListLinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SideBarListLinkComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBarListLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
