import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Employee } from '../../models/employee.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';


@Component({
  selector: 'app-employee-update-modal',
  templateUrl: './employee-update-modal.component.html',
  styleUrl: './employee-update-modal.component.css'
})
export class EmployeeUpdateModalComponent {

  @Input() employee: Employee | null = null; // Recibe el empleado desde el componente padre
  @Output() employeeUpdated = new EventEmitter<void>();
  
  employeeForm!: FormGroup;
  display: boolean = false;
  
  uploadedFile: boolean = false;
  photoUrl: string | null = null;
  visible: boolean = false;
  
  constructor(private fb: FormBuilder, private employeeService: EmployeeService) {}

  ngOnInit() {
    
    this.employeeForm = this.fb.group({
      firstName: [this.employee?.firstName || '', Validators.required],
      lastName: [this.employee?.lastName || '', Validators.required],
      email: [this.employee?.email || '', [Validators.required, Validators.email]],
      position: [this.employee?.position || '', Validators.required],
      hireDate: [this.employee?.hireDate ? new Date(this.employee.hireDate) : '', Validators.required],
      contractEndDate: [this.employee?.contractEndDate || ''],
      photoUrl: [this.employee?.photoUrl || '']
    });
  }

  ngOnChanges() {
    if (this.employee) {
      this.updateFormWithEmployeeData(this.employee);
    }
  }

  updateFormWithEmployeeData(employee: Employee) {
    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      position: employee.position,
      hireDate: employee.hireDate ? new Date(employee.hireDate) : '',
      contractEndDate: employee.contractEndDate,
      photoUrl: employee.photoUrl
    });
  }

  onSubmit(): void {
    if (this.employeeForm.valid) {
      console.log('Formulario enviado:', this.employeeForm.value);
    }
  }

  editEmployee() {
    if (this.employeeForm.valid) {
      const employeeData = this.employeeForm.value;
  
      if (this.employee) {
        const employeeId = this.employee.employeeId; // Obtén el ID del empleado
        this.updateEmployee(employeeId, employeeData);
      } else {
        console.log('Empleado no encontrado');
      }
    } else {
      console.log('Formulario inválido');
    }
  }

  updateEmployee(employeeId: number, employee: Employee) {
    this.employeeService.updateEmployee(employeeId, employee).subscribe(
      (updatedEmployee) => {
        console.log('Empleado actualizado:', updatedEmployee);
        this.display = false;
        this.employeeForm.reset();
        this.photoUrl = null;
        this.uploadedFile = false;

        this.employeeUpdated.emit();
        
      },
      (error) => {
        console.error('Error al actualizar el empleado:', error);
      }
    );
  }

  onFileSelect(event: any) {
    const file = event.files[0];
    if (file) {
      this.uploadedFile = true;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.photoUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  show() {
    console.log('Hijo', this.employee);
    this.display = true;
  }

  close() {
      this.display = false;
  }

}
