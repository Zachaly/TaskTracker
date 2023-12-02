import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import UserTaskStatusModel from 'src/app/model/user-task-status/UserTaskStatusModel';
import { faTrash, faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-task-status-list-item',
  templateUrl: './task-status-list-item.component.html',
  styleUrls: ['./task-status-list-item.component.css']
})
export class TaskStatusListItemComponent {
  @Input() status: UserTaskStatusModel = {
    id: 0,
    name: '',
    color: '',
    index: 0,
    isDefault: false
  }

  faTrash = faTrash
  faAngleUp = faAngleUp
  faAngleDown = faAngleDown

  @Output() moveIndexUp: EventEmitter<any> = new EventEmitter()
  @Output() moveIndexDown: EventEmitter<any> = new EventEmitter()
  @Output() updateName: EventEmitter<string> = new EventEmitter()
  @Output() updateColor: EventEmitter<string> = new EventEmitter()
  @Output() deleteStatus: EventEmitter<any> = new EventEmitter()

  isUpdatingName = false
  isUpdatingColor = false

  updatedName = ''
  updatedColor = ''

  toggleIsUpdatingName() {
    this.isUpdatingName = !this.isUpdatingName

    if (this.isUpdatingName) {
      this.updatedName = this.status.name
    }
  }

  toggleIsUpdatingColor() {
    this.isUpdatingColor = !this.isUpdatingColor

    if (this.isUpdatingColor) {
      this.updatedColor = this.status.color
    }
  }

  confirmNameUpdate() {
    this.updateName.emit(this.updatedName)
    this.toggleIsUpdatingName()
  }

  confirmColorUpdate() {
    this.updateColor.emit(this.updatedColor)
    this.toggleIsUpdatingColor()
  }
}
