import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-position',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzIconModule, NzDividerModule, NzButtonModule],
  templateUrl: './position.component.html',
  styleUrl: './position.component.scss'
})
export class PositionComponent {

  position: any;
  companyId: number = 0;

  constructor(
    private router: Router,
  ){}

  onCreate(): void {
    this.router.navigateByUrl('position/detail/0')
  }

  onDelete() {

  }
}
