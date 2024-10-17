import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'manageEmployeesApp';
  items: MenuItem[] | undefined;
  
  ngOnInit() {
    this.items = [
        {
            label: 'Empleados',
            icon: 'pi pi-list',
            routerLink: '/employees'
        },
        {
            label: 'Nuevo empleado',
            icon: 'pi pi-plus',
            routerLink: '/add-employee'
        },
    ]
  }
}
