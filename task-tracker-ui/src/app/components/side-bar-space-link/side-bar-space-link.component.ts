import { Component, Input } from '@angular/core';
import { faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';
import UserSpaceModel from 'src/app/model/user-space/UserSpaceModel';

@Component({
  selector: 'app-side-bar-space-link',
  templateUrl: './side-bar-space-link.component.html',
  styleUrls: ['./side-bar-space-link.component.css']
})
export class SideBarSpaceLinkComponent {
  @Input() space: UserSpaceModel = {
    id: 0,
    lists: [],
    title: '',
    statusGroup: {
      id: 0,
      name: '',
      statuses: [],
      isDefault: false
    },
    owner: {
      id: 0,
      firstName: '',
      lastName: '',
      email: ''
    }
  }

  isExpanded: boolean = false

  currentIcon = faAngleDown

  toggleIsExpanded() {
    this.isExpanded = !this.isExpanded

    if (this.isExpanded) {
      this.currentIcon = faAngleUp
    }
    else {
      this.currentIcon = faAngleDown
    }
  }
}
