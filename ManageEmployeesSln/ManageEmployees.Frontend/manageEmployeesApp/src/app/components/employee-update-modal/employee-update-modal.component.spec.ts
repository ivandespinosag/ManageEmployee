import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeUpdateModalComponent } from './employee-update-modal.component';

describe('EmployeeUpdateModalComponent', () => {
  let component: EmployeeUpdateModalComponent;
  let fixture: ComponentFixture<EmployeeUpdateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EmployeeUpdateModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
