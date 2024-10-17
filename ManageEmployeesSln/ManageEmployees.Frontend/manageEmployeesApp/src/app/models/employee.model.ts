export interface Employee {
  employeeId: number;
  firstName: string;
  lastName: string;
  position: string;
  email: string;
  hireDate: Date;
  photoUrl?: string;
  contractEndDate?: Date;
}

