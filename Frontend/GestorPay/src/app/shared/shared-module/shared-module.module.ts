import { CommonModule } from '@angular/common';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { NgzorroModule } from './ngzorro.module';
import { PipeModule } from './pipe.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterOutlet,
    NgxMaskDirective,
    NgxMaskPipe,

    ///ngzorro
    NgzorroModule,

    //pipe
    PipeModule
  ],
  exports: [
    CommonModule,
    RouterOutlet,
    ReactiveFormsModule,
    FormsModule,
    NgxMaskDirective,
    NgxMaskPipe,

    //ngzorro
    NgzorroModule,

    //pipe
    PipeModule
  ],
  schemas: [],
  providers: [
    provideNgxMask(),
    { provide: LOCALE_ID, useValue: 'pt' },
  ],
})
export class SharedModuleModule { }
