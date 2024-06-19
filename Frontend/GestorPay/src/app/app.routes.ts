import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/dashboard' },
  { path: 'dashboard', loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES) },
  { path: 'expense-manager', loadChildren: () => import('./pages/expense-manager/expense-manager.routes').then(m => m.EXPENSEMANAGER_ROUTES) },
  { path: 'position', loadChildren: () => import('./pages/position/position.routes').then(m => m.POSITION_ROUTES) }
];
