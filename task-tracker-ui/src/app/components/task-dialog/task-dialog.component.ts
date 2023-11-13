import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { tap } from 'rxjs';
import UserTaskModel from 'src/app/model/UserTaskModel';
import UpdateUserTaskRequest from 'src/app/model/request/UpdateUserTaskRequest';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-task-dialog',
  templateUrl: './task-dialog.component.html',
  styleUrls: ['./task-dialog.component.css']
})
export class TaskDialogComponent implements OnInit {
  @Input() id: number = 0
  @Output() close: EventEmitter<any> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() taskUpdated: EventEmitter<UserTaskModel> = new EventEmitter()

  task: UserTaskModel = {
    id: 0,
    title: '',
    description: '',
    creationTimestamp: 0,
    creator: { id: 0, firstName: '', lastName: '', email: '' }
  }

  isUpdating = false

  updateRequest: UpdateUserTaskRequest = {
    id: 0
  }

  creationDate = () => new Date(this.task.creationTimestamp)
  dueDate = () => new Date(this.task.dueTimestamp!)

  constructor(private taskService: UserTaskService) {

  }

  ngOnInit(): void {
    this.loadTask().subscribe()
  }

  private loadTask() {
    return this.taskService.getById(this.id).pipe(tap(res => {
      this.task = res
      const { id, description, title, dueTimestamp } = res
      this.updateRequest = { id, description, title, dueTimestamp }
    }))
  }

  public onDeleteTask() {
    this.deleteTask.emit(this.task.id)
    this.close.emit()
  }

  public updateTask() {
    this.taskService.update(this.updateRequest).subscribe(async () => {
      this.isUpdating = false
      this.loadTask().subscribe(res => this.taskUpdated.emit(res))
    })
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.updateRequest.dueTimestamp = date.getTime()
  }
}
