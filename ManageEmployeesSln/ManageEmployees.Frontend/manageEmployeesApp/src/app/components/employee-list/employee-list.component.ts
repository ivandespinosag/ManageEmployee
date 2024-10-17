import { Component, OnInit, ViewChild } from '@angular/core';
import { Employee } from '../../models/employee.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeUpdateModalComponent } from '../employee-update-modal/employee-update-modal.component';
import { Observable } from 'rxjs';
import { EmployeeStateService } from '../../services/employee-state.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent implements OnInit {

  employees$: Observable<Employee[]>;
  visible: boolean = false;

  @ViewChild('updateModal') updateModal!: EmployeeUpdateModalComponent;

  selectedEmployee!: Employee;

  employeeForm!: FormGroup;  
  uploadedFile: boolean = false;
  // photoUrl: string | null = null;

  constructor(private employeeStateService: EmployeeStateService, private fb: FormBuilder) {
    this.employees$ = this.employeeStateService.employees$;
  }

  ngOnInit(): void {

    this.employeeStateService.loadEmployees();

    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      position: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      hireDate: [null, Validators.required],
      photoUrl: [null, Validators.required]
    });
    
  }

  deleteEmployee(employeeId: number) {
    console.log('employeeId', employeeId);
    
    if (confirm('¿Estás seguro de que deseas eliminar este empleado?')) {
      this.employeeStateService.deleteEmployee(employeeId);
    }
  }



  showUpdateDialog(employee: Employee) {
    console.log('dialog', employee);
    
    this.selectedEmployee = employee;
    this.updateModal.show();
    
  }

  onEmployeeUpdated() {
    this.employeeStateService.loadEmployees(); // Vuelve a cargar los empleados
  }



}
