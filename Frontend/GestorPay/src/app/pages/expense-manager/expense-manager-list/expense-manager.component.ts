import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-expense-manager',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzIconModule, NzDividerModule, NzButtonModule],
  templateUrl: './expense-manager.component.html',
  styleUrl: './expense-manager.component.scss'
})
export class ExpenseManagerComponent implements OnInit {

  manager: any;
  companyId: number = 0;

  constructor(
    private router: Router,
  ){}

  ngOnInit(): void {

  }

  getExpenseManager(): void {

  }

  onCreate(): void {
    this.router.navigateByUrl('expense-manager/detail/0');
  }

  onDelete() {
  }

}
