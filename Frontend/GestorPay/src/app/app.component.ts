import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import format from 'date-fns/format';
import { ptBR } from 'date-fns/locale';
import { filter } from 'rxjs';
import { SharedModuleModule } from './shared/shared-module/shared-module.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SharedModuleModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isCollapsed = false;
  currentDate: string = '';
  isHideSideBar: boolean = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.currentDate = format(new Date(), "dd 'de' MMMM 'de' yyyy", { locale: ptBR });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Verificar se a URL contém "login" ou "not-found"
      const currentRoute = this.activatedRoute.snapshot.firstChild?.routeConfig?.path;
      this.isHideSideBar = currentRoute === 'login' || currentRoute === 'not-found' || currentRoute === 'register-company';
    });
  }
}
