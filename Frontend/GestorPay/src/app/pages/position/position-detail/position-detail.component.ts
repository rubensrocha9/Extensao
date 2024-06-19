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

@Component({
  selector: 'app-position-detail',
  standalone: true,
  imports: [CommonModule, NzDividerModule, NzButtonModule, ReactiveFormsModule, FormsModule, NzFormModule, NzSelectModule, NzInputModule, NzModalModule],
  templateUrl: './position-detail.component.html',
  styleUrl: './position-detail.component.scss'
})
export class PositionDetailComponent implements OnInit {

  headerTitle: string = '';
  form!: FormGroup;

  constructor(
    private router: Router,
    private modal: NzModalService,
    private formBuilder: FormBuilder,
  ){
    this.form = this.formBuilder.group({
      name: [null , [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.headerTitle = 'Criar Cargo';
  }

  onSubmit() {
  }


  onBack(): void {
    this.modal.confirm({
      nzTitle: '<i>Certeza que deseja voltar?</i>',
      nzOnOk: () => this.router.navigateByUrl('position')
    });
  }
}
