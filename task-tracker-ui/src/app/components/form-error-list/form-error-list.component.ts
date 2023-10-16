import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form-error-list',
  templateUrl: './form-error-list.component.html',
  styleUrls: ['./form-error-list.component.css']
})
export class FormErrorListComponent {
  @Input() errors?: string[]
}
