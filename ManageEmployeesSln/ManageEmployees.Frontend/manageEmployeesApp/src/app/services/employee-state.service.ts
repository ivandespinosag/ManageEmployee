import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { Employee } from '../models/employee.model';
import { EmployeeService } from './employee.service';
import { EmployeeAdd } from '../models/employee-add.model';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmployeeStateService {

  private employeesSubject = new BehaviorSubject<Employee[]>([]);
  employees$: Observable<Employee[]> = this.employeesSubject.asObservable();

  constructor(private employeeService: EmployeeService) { 
    this.loadEmployees();
  }

  loadEmployees() {
    this.employeeService.getEmployees().pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Error al cargar los empleados:', error);
        alert('No se pueden cargar los empleados.');
        return throwError(() => new Error('Error al cargar los empleados'));
      })
    ).subscribe((data: Employee[]) => {
      this.employeesSubject.next(data);
    });
  }

  deleteEmployee(employeeId: number): void {
    this.employeeService.deleteEmployee(employeeId).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Error al eliminar el empleado:', error);
        alert('Error al eliminar el empleado.');
        return throwError(() => new Error('Error al eliminar el empleado'));
      })
    ).subscribe(() => {
      this.loadEmployees(); // Recargar empleados después de la eliminación
    });
  }

  addEmployee(employee: EmployeeAdd): Observable<EmployeeAdd> {    
    if (!this.validateEmployee(employee)) {
      alert('Error al validar los datos.');
      return throwError(() => new Error('Error al validar los datos'));
    }

    return this.employeeService.addEmployee(employee).pipe(
      tap((addedEmployee: EmployeeAdd) => {
        const newEmployee: Employee = {
          ...addedEmployee,
          employeeId: 0 // Me falta asignar el employeeId que genera la base de datos al insertar
        };
        // Actualiza el estado de la lista de empleados después de agregar
        const currentEmployees = this.employeesSubject.getValue();
        this.employeesSubject.next([...currentEmployees, newEmployee]);
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Error al agregar el empleado:', error);
        alert('Error al agregar el empleado');
        return throwError(() => new Error('Error al agregar el empleado'));
      })
    );
  }

  private validateEmployee(employee: EmployeeAdd): boolean {
    if (!employee.firstName || !employee.lastName || !employee.position || !employee.photoUrl) {
      return false; // Datos incompletos
    }    
    return true;
  }
  
}
