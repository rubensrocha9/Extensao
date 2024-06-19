import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import format from 'date-fns/format';
import { ptBR } from 'date-fns/locale';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { filter } from 'rxjs';
import { AuthService } from './core/service/auth.service';
import { LoaderComponent } from "./shared/components/loader/loader.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [CommonModule, RouterOutlet, NzIconModule, NzLayoutModule, NzMenuModule, RouterLink, LoaderComponent, NzButtonModule]
})
export class AppComponent implements OnInit {
  isCollapsed = false;
  currentDate: string = '';
  isHideSideBar: boolean = false;
  isAdmin: boolean = false;
  isCompany: boolean = false;
  companyId: number = 0;
  employeeId: number = 0;

  constructor (
    private router: Router,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.currentDate = format(new Date(), "dd 'de' MMMM 'de' yyyy", { locale: ptBR });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Verificar se a URL contém "login" ou "not-found"
      const currentRoute = this.activatedRoute.snapshot.firstChild?.routeConfig?.path;
      this.isHideSideBar =
        currentRoute === 'login' ||
        currentRoute === 'not-found' ||
        currentRoute === 'register-company' ||
        currentRoute === 'confirm-email' ||
        currentRoute === 'register-employee';
    });
  }

  onLogout(): void {
    this.authService.signOut();
  }
}
