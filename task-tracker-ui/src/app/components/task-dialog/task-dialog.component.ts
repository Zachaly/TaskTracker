import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faBan, faFlag } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import UserTaskModel from 'src/app/model/user-task/UserTaskModel';
import UserTaskStatusModel from 'src/app/model/user-task-status/UserTaskStatusModel';
import UserTaskPriority from 'src/app/model/enum/UserTaskPriority';
import UpdateUserTaskRequest from 'src/app/model/user-task/UpdateUserTaskRequest';
import { UserTaskService } from 'src/app/services/user-task.service';
import UserModel from 'src/app/model/user/UserModel';
import { TaskFileAttachmentService } from 'src/app/services/task-file-attachment.service';

@Component({
  selector: 'app-task-dialog',
  templateUrl: './task-dialog.component.html',
  styleUrls: ['./task-dialog.component.css']
})
export class TaskDialogComponent implements OnInit {
  @Input() id: number = 0
  @Input() spaceId: number = 0
  @Input() statuses: UserTaskStatusModel[] = []
  @Output() close: EventEmitter<any> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() taskUpdated: EventEmitter<UserTaskModel> = new EventEmitter()

  task: UserTaskModel = {
    id: 0,
    title: '',
    description: '',
    creationTimestamp: 0,
    creator: { id: 0, firstName: '', lastName: '', email: '' },
    status: { id: 0, index: 0, name: '', color: '', isDefault: false },
    assignedUsers: [],
    listId: 0,
    attachments: []
  }

  faFlag = faFlag
  faBan = faBan

  isUpdating = false

  priorityLevels = UserTaskPriority

  isUpdatingStatus = false
  isUpdatingPriority = false
  isManagingAssignedUsers = false

  updateRequest: UpdateUserTaskRequest = {
    id: 0
  }

  creationDate = () => new Date(this.task.creationTimestamp)
  dueDate = () => new Date(this.task.dueTimestamp!)

  constructor(private taskService: UserTaskService, private taskFileAttachmentService: TaskFileAttachmentService) {

  }

  ngOnInit(): void {
    this.loadTask().subscribe()
  }

  private loadTask() {
    return this.taskService.getById(this.id).pipe(tap(res => {
      this.task = res
      const { id, description, title, dueTimestamp, priority } = res
      this.updateRequest = { id, description, title, dueTimestamp, statusId: res.status.id, priority }
    }))
  }

  public onDeleteTask() {
    this.deleteTask.emit(this.task.id)
    this.close.emit()
  }

  public updateTask() {
    this.taskService.update(this.updateRequest).subscribe(() => {
      this.isUpdating = false
      this.isUpdatingStatus = false
      this.loadTask().subscribe(res => this.taskUpdated.emit(res))
    })
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.updateRequest.dueTimestamp = date.getTime()
  }

  selectStatus(statusId: number) {
    this.updateRequest.statusId = statusId

    this.updateTask()

    this.isUpdatingStatus = false
  }

  confirmPriorityUpdate(priority?: UserTaskPriority) {
    this.updateRequest.priority = priority

    this.updateTask()

    this.isUpdatingPriority = false
  }

  assignedUserDeleted(id: number) {
    this.task.assignedUsers = this.task.assignedUsers.filter(x => x.id !== id)
  }

  assignedUserAdded(user: UserModel) {
    this.task.assignedUsers.push(user)
  }


  priorityColor(priority?: UserTaskPriority) {
    if (priority === undefined || priority === null) {
      return '#fafafa'
    }

    if (priority == UserTaskPriority.urgent) {
      return '#e02626'
    }
    else if (priority == UserTaskPriority.medium) {
      return '#0bcde3'
    }
    else if (priority == UserTaskPriority.high) {
      return '#e7ed32'
    }
    else if (priority == UserTaskPriority.low) {
      return '#7f8485'
    }

    return '#ad0505'
  }

  deleteAttachedFile(id: number) {
    this.taskFileAttachmentService.deleteById(id).subscribe(() => this.task.attachments = this.task.attachments.filter(x => x.id !== id))
  }

  attachFiles(e: Event) {
    const target = e.target as HTMLInputElement

    const files: File[] = []

    for(let i = 0; i < target.files!.length; i++) {
      files.push(target.files![i])
    }

    this.taskFileAttachmentService.post(this.task.id, files).subscribe(() => {
      this.taskFileAttachmentService.get({ taskId: this.task.id }).subscribe(res => this.task.attachments = res)
    })
  }
}
