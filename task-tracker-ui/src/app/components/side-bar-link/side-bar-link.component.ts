import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-side-bar-link',
  templateUrl: './side-bar-link.component.html',
  styleUrls: ['./side-bar-link.component.css']
})
export class SideBarLinkComponent {
  @Input() link = ''
  @Input() isActive = false
  @Input() name = ''
}
