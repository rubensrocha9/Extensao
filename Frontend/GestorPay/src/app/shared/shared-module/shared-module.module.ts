import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { LoaderComponent } from '../components/loader/loader.component';
import { ZorroModule } from './zorro.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NgxMaskDirective,
    NgxMaskPipe,

    ReactiveFormsModule,
    FormsModule,

    ZorroModule,

    LoaderComponent
  ],
  exports: [
    CommonModule,
    NgxMaskDirective,
    NgxMaskPipe,

    ReactiveFormsModule,
    FormsModule,

    ZorroModule,

    LoaderComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    provideNgxMask(),
    { provide: LOCALE_ID, useValue: 'pt' },
  ],
})
export class SharedModuleModule { }
