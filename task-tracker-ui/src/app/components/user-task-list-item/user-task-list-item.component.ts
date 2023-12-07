import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import UserTaskModel from 'src/app/model/user-task/UserTaskModel';
import UserTaskStatusModel from 'src/app/model/user-task-status/UserTaskStatusModel';
import { faTrash, faEllipsis, faFlag, faBan, faPencil } from '@fortawesome/free-solid-svg-icons';
import UserTaskPriority, { priorityColor } from 'src/app/model/enum/UserTaskPriority';
import UpdateUserTaskRequest from 'src/app/model/user-task/UpdateUserTaskRequest';

@Component({
  selector: '[app-user-task-list-item]',
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
  faPencil = faPencil
  priorityLevels = UserTaskPriority
  priorityColor = priorityColor

  @Input() statuses: UserTaskStatusModel[] = []

  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() updateRequested: EventEmitter<UpdateUserTaskRequest> = new EventEmitter()

  showDialog = false
  isUpdatingTitle = false
  isUpdatingDueTimestamp = false
  isUpdatingStatus = false
  isUpdatingPriority = false

  updatedTitle = ''
  updatedDueTimestamp?: number = undefined

  updateRequest: UpdateUserTaskRequest = {
    id: 0
  }

  creationTimestamp = () => new Date(this.task.creationTimestamp)
  dueTimestamp = () => new Date(this.task.dueTimestamp!)

  ngOnInit(): void {
    this.resetUpdateRequest()
  }

  resetUpdateRequest() {
    const { id, priority, dueTimestamp, title, description } = this.task

    this.updateRequest = {
      id,
      priority,
      dueTimestamp,
      title,
      statusId: this.task.status.id,
      description: description
    }
  }

  confirmUpdateTitle() {
    this.isUpdatingTitle = false
    this.confirmUpdate()
  }

  confirmUpdateDueTimestamp() {
    this.isUpdatingDueTimestamp = false

    this.confirmUpdate()
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.updateRequest.dueTimestamp = date.getTime()
  }

  updateTask(task: UserTaskModel) {
    const { title, description, dueTimestamp, priority } = task
    this.task.description = description
    this.task.title = title
    this.task.dueTimestamp = dueTimestamp
    this.task.status = task.status
    this.task.priority = priority

    this.resetUpdateRequest()
  }

  toggleIsUpdatingStatus() {
    this.isUpdatingStatus = !this.isUpdatingStatus && this.statuses.length > 0
  }

  confirmStatusUpdate(statusId: number) {
    this.updateRequest.statusId = statusId

    this.confirmUpdate()

    this.toggleIsUpdatingStatus()
  }

  confirmPriorityUpdate(priority?: UserTaskPriority) {
    this.isUpdatingPriority = false

    this.updateRequest.priority = priority

    this.confirmUpdate()
  }

  confirmUpdate() {
    this.updateRequested.emit(this.updateRequest)
  }
}
