import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { LoginComponent } from './root/login/login.component';
import { NotFoundComponent } from './root/not-found/not-found.component';
import { RegisterCompanyComponent } from './root/register-company/register-company.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'register-company', component: RegisterCompanyComponent },
  // { path: 'login', component: LoginComponent },

  { path: '**', redirectTo: 'not-found' },
  { path: 'not-found', component: NotFoundComponent },

  {
    canActivate: [AuthGuard],
    path: '',
    children: [
      { path: '', pathMatch: 'full', redirectTo: '/dashboard' },
      { path: 'dashboard', data: { role: 'Admin' }, loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES) },
    ]
  }
];
