import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { take } from 'rxjs';
import { AuthService } from '../../core/service/auth.service';
import { RegisterEmployee } from '../../shared/models/auth';
import { LoaderService } from '../../shared/service/loader.service';
import { ModalService } from '../../shared/service/modal.service';
import { SharedModuleModule } from '../../shared/shared-module/shared-module.module';

@Component({
  selector: 'app-register-employee',
  standalone: true,
  imports: [SharedModuleModule],
  templateUrl: './register-employee.component.html',
  styleUrl: './register-employee.component.scss'
})
export class RegisterEmployeeComponent {

  passwordVisible = false;
  checkPasswordVisible = false;
  companyId: number = 0;
  employeeId: number = 0;
  registerForm!: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private message: NzMessageService,
    private loaderService: LoaderService,
    private notificationService: ModalService
  )
  {
    this.registerForm = this.formBuilder.group({
      documentNumber: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required]],
      checkPassword: [null, [Validators.required]],
    });
  }

  onRegister(): void {
    const formValues = this.registerForm.getRawValue();
    const register: RegisterEmployee = {
      email: formValues.email,
      password: formValues.password,
      documentNumber: formValues.documentNumber
    }

    LoaderService.toggle({ show: true });
    this.authService.signUpEmployee(this.companyId, this.employeeId, register).pipe(take(1)).subscribe(
      data => {
        LoaderService.toggle({ show: false });
        this.message.success('Cadastro finalizado com sucesso!', { nzDuration: 3000 });
        this.onLogin();
      }, error => {
        LoaderService.toggle({ show: false });
        this.notificationService.modalLoadDataError(error);
    });
  }

  onLogin() {
    this.router.navigateByUrl('login');
  }
}
