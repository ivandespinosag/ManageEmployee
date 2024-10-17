import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Employee } from '../models/employee.model';
import { EmployeeAdd } from '../models/employee-add.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7197/api/employee';  // URL del API en .NET Core

  constructor(private http: HttpClient) {}

  // Obtener todos los empleados
  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/GetEmployees`).pipe(
      catchError(this.handleError)  // Manejo de errores
    );
  }

  // Obtener un empleado por ID
  getEmployee(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Añadir un empleado
  addEmployee(employee: EmployeeAdd): Observable<EmployeeAdd> {
    if (!this.validateEmployee(employee)) {
      return throwError(() => new Error('Invalid employee data'));
    }

    console.log('addEmployee', employee);
    return this.http.post<EmployeeAdd>(`${this.apiUrl}/PostEmployee`, employee).pipe(
      catchError(this.handleError)
    );
  }

  // Actualizar un empleado
  updateEmployee(employeeId: number, employee: Employee): Observable<Employee> {
    if (!this.validateEmployee(employee)) {
      return throwError(() => new Error('Invalid employee data for update'));
    }

    return this.http.put<Employee>(`${this.apiUrl}/PutEmployee/${employeeId}`, employee).pipe(
      catchError(this.handleError)
    );
  }

  // Eliminar un empleado
  deleteEmployee(employeeId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteEmployee/${employeeId}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Erro desconocido';
    
    if (error.error instanceof ErrorEvent) {
      // Error del lado del cliente
      errorMessage = `Error lado del cliente: ${error.error.message}`;
    } else {
      // Error del lado del servidor
      errorMessage = `Error lado servidor: ${error.status} - ${error.message}`;
    }

    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

  private validateEmployee(employee: EmployeeAdd): boolean {
    if (!employee.firstName || !employee.lastName || !employee.position) {
      console.error('validateEmployee, campos requeridos');
      return false;
    }
    
    return true;
  }
  
}