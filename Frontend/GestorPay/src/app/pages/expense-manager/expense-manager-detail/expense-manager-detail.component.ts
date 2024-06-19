import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ExpenseManagerService } from '../../../core/service/expense-manager.service';

@Component({
  selector: 'app-expense-manager-detail',
  standalone: true,
  imports: [CommonModule, NzDividerModule, NzButtonModule, ReactiveFormsModule, FormsModule, NzFormModule, NzSelectModule, NzInputModule, NzModalModule],
  templateUrl: './expense-manager-detail.component.html',
  styleUrl: './expense-manager-detail.component.scss'
})
export class ExpenseManagerDetailComponent implements OnInit {

  headerTitle: string = '';
  form!: FormGroup;

  constructor(
    private router: Router,
    private modal: NzModalService,
    private formBuilder: FormBuilder,
    private expenseService: ExpenseManagerService
  ) {
    this.form = this.formBuilder.group({
      name: [null , [Validators.required]],
      amount: [null , [Validators.required]],
      status: [null , [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.headerTitle = 'Criar Gasto';
  }

  onSubmit(): void {

  }

  onBack(): void {
    this.modal.confirm({
      nzTitle: '<i>Certeza que deseja voltar?</i>',
      nzOnOk: () => this.router.navigateByUrl('expense-manager')
    });
  }
}
