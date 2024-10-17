import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { emailValidator } from '../../shared/email.validator';
import { UploadEvent } from 'primeng/fileupload';
import { EmployeeAdd } from '../../models/employee-add.model';
import { EmployeeStateService } from '../../services/employee-state.service';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.css'
})
export class EmployeeFormComponent implements OnInit{

  employeeForm: FormGroup;
  uploadedFile: File | null = null;
  photoUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private employeeStateService: EmployeeStateService,
    private router: Router
  ) {
    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      position: ['', Validators.required],
      email: ['', [Validators.required, emailValidator()]],
      hireDate: ['', Validators.required],
      photoUrl: ['']
    });
  }

  ngOnInit(): void {}

  saveEmployee(): void {
    if (this.employeeForm.valid) {
      const employeeAdd: EmployeeAdd = {
        ...this.employeeForm.value,
        photoUrl: this.photoUrl,
      }
      
      this.employeeStateService.addEmployee(employeeAdd).subscribe(() => {
        this.employeeStateService.loadEmployees();
        this.router.navigate(['/employees']);
      });
    }
  }

  onUpload(event: UploadEvent) {
    console.log('event', event);
    
    // this.messageService.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded with Basic Mode' });
  }

  onFileSelect(event: any): void {
    const file: File = event.files[0];
    
    if (file && file.type === 'image/jpeg') {
      this.uploadedFile = file;
      this.photoUrl = URL.createObjectURL(file);
    } else {
      // Si no es un archivo JPG...
      this.uploadedFile = null;
      this.photoUrl = null;
      alert('Solo se permiten archivos JPG');
    }
  }

}
