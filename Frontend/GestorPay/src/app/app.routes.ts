import { Routes } from '@angular/router';
import { CompanyRegisterComponent } from './root/company-register/company-register.component';
import { LoginComponent } from './root/login/login.component';
import { NotFoundComponent } from './root/not-found/not-found.component';
import { RegisterEmployeeComponent } from './root/register-employee/register-employee.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register-company', component: CompanyRegisterComponent },
  { path: 'register-employee', component: RegisterEmployeeComponent },


  { path: '', pathMatch: 'full', redirectTo: '/dashboard' },
  { path: 'dashboard', loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES) },
  { path: 'expense-manager', loadChildren: () => import('./pages/expense-manager/expense-manager.routes').then(m => m.EXPENSEMANAGER_ROUTES) },
  { path: 'position', loadChildren: () => import('./pages/position/position.routes').then(m => m.POSITION_ROUTES) },
  { path: 'employee', loadChildren: () => import('./pages/employee/employee.routes').then(m => m.EMPLOYEE_ROUTES) },
  { path: 'permission', loadChildren: () => import('./pages/permission/permission.routes').then(m => m.PERMISSION_ROUTES) },

  { path: '**', pathMatch: 'full', redirectTo: 'not-found' },
  { path: 'not-found', component: NotFoundComponent },
];
