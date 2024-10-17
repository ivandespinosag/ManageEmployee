import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

// Validador personalizado para un email con dominio completo
export function emailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const email = control.value;
    // Expresión regular para validar el formato del correo electrónico y que tenga un dominio completo
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    
    return email && !emailRegex.test(email) ? { invalidEmail: true } : null;
  };
}