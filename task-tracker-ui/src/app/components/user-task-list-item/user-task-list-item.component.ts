import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import UserTaskModel from 'src/app/model/UserTaskModel';
import UserTaskStatusModel from 'src/app/model/UserTaskStatusModel';
import { faTrash, faEllipsis, faFlag, faBan } from '@fortawesome/free-solid-svg-icons';
import UserTaskPriority from 'src/app/model/enum/UserTaskPriority';

@Component({
  selector: 'app-user-task-list-item',
  templateUrl: './user-task-list-item.component.html',
  styleUrls: ['./user-task-list-item.component.css']
})
export class UserTaskListItemComponent implements OnInit {

  @Input() task: UserTaskModel = {
    id: 0,
    creationTimestamp: 0,
    title: '',
    description: '',
    creator: { id: 0, firstName: '', lastName: '', email: '' },
    status: { id: 0, index: 0, name: '', color: '', isDefault: false }
  }

  faTrash = faTrash
  faEllipsis = faEllipsis
  faFlag = faFlag
  faBan = faBan

  priorityLevels = UserTaskPriority

  @Input() statuses: UserTaskStatusModel[] = []

  @Output() updateTitle: EventEmitter<string> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() updateDueTimestamp: EventEmitter<number | undefined> = new EventEmitter()
  @Output() updateStatus: EventEmitter<number> = new EventEmitter()
  @Output() updatePriority: EventEmitter<UserTaskPriority | undefined> = new EventEmitter()
  @Output() taskUpdated: EventEmitter<UserTaskModel> = new EventEmitter()  

  showDialog = false
  isUpdatingTitle = false
  isUpdatingDueTimestamp = false
  isUpdatingStatus = false
  isUpdatingPriority = false

  updatedTitle = ''
  updatedDueTimestamp?: number = undefined

  creationTimestamp = () => new Date(this.task.creationTimestamp)
  dueTimestamp = () => new Date(this.task.dueTimestamp!)

  ngOnInit(): void {
    this.updatedTitle = this.task.title
    this.updatedDueTimestamp = this.task.dueTimestamp
  }

  confirmUpdateTitle() {
    this.isUpdatingTitle = false
    this.updateTitle.emit(this.updatedTitle)
  }

  confirmUpdateDueTimestamp() {
    this.isUpdatingDueTimestamp = false
    this.updateDueTimestamp.emit(this.updatedDueTimestamp)
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.updatedDueTimestamp = date.getTime()
  }

  updateTask(task: UserTaskModel) {
    const { title, description, dueTimestamp } = task
    this.task.description = description
    this.task.title = title
    this.task.dueTimestamp = dueTimestamp
    this.task.status = task.status
  }

  toggleIsUpdatingStatus(){
    this.isUpdatingStatus = !this.isUpdatingStatus && this.statuses.length > 0
  }

  confirmStatusUpdate(statusId: number){
    this.updateStatus.emit(statusId)

    this.toggleIsUpdatingStatus()
  }

  priorityColor(priority?: UserTaskPriority) {
    if(priority === undefined || priority === null) {
      return '#fafafa'
    }

    if(priority == UserTaskPriority.urgent) {
      return '#e02626'
    }
    else if(priority == UserTaskPriority.medium) {
      return '#0bcde3'
    }
    else if(priority == UserTaskPriority.high) {
      return '#e7ed32'
    }
    else if(priority == UserTaskPriority.low) {
      return '#7f8485'
    }

    return '#ad0505'
  }

  confirmPriorityUpdate(priority?: UserTaskPriority) {
    this.isUpdatingPriority = false

    this.updatePriority.emit(priority)
  }
}
