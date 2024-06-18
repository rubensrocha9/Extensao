import { Component } from '@angular/core';
import { SharedModuleModule } from '../../shared/shared-module/shared-module.module';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [SharedModuleModule],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss'
})
export class NotFoundComponent {


  onBack() {

  }

}
