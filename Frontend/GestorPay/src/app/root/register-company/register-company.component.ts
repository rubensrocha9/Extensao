import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedModuleModule } from '../../shared/shared-module/shared-module.module';

@Component({
  selector: 'app-register-company',
  standalone: true,
  imports: [SharedModuleModule],
  templateUrl: './register-company.component.html',
  styleUrl: './register-company.component.scss'
})
export class RegisterCompanyComponent {

  passwordVisible = true;
  registerForm!: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder
  )
  {
    this.registerForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]],
      documentNumber: [null, [Validators.required]],
      password: [null, [Validators.required]],
      checkPassword: [null, [Validators.required]],
    });
  }

  onRegister(): void {

  }

  onLogin() {
    this.router.navigateByUrl('login');
  }

}
