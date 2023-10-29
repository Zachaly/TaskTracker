import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserTaskModel from 'src/app/model/UserTaskModel';

@Component({
  selector: 'app-user-task-list-item',
  templateUrl: './user-task-list-item.component.html',
  styleUrls: ['./user-task-list-item.component.css']
})
export class UserTaskListItemComponent {
  @Input() task: UserTaskModel = { 
    id: 0,
    creationTimestamp: 0,
    title: '',
    description: '',
    creator: { id: 0, firstName: '', lastName: '', email: '' }
  }

  showDialog = false;

  @Output() deleteTask: EventEmitter<number> = new EventEmitter()

  creationTimestamp = () => new Date(this.task.creationTimestamp)
  dueTimestamp = () => new Date(this.task.dueTimestamp!)
}
