import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarSpaceLinkComponent } from './side-bar-space-link.component';

describe('SideBarSpaceLinkComponent', () => {
  let component: SideBarSpaceLinkComponent;
  let fixture: ComponentFixture<SideBarSpaceLinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SideBarSpaceLinkComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBarSpaceLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
