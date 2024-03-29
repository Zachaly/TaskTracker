import { Component, EventEmitter, Input, Output } from '@angular/core';
import AddUserTaskRequest from 'src/app/model/user-task/AddUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-add-task-form',
  templateUrl: './add-task-form.component.html',
  styleUrls: ['./add-task-form.component.css']
})
export class AddTaskFormComponent {
  @Input() isAddingTask = false
  @Input() listId?: number
  @Input() creatorId: number = 0
  @Input() statusId: number = 0
  @Input() spaceId: number = 0
  @Output() taskAdded: EventEmitter<any> = new EventEmitter()

  newTask: AddUserTaskRequest = {
    creatorId: 0,
    title: '',
    description: '',
    statusId: 0
  }

  constructor(private taskService: UserTaskService, private authService: AuthService) {

  }

  cancelAddingTask() {
    this.newTask = {
      title: '',
      description: '',
      creatorId: 0,
      listId: 0,
      statusId: this.statusId
    }
    this.isAddingTask = false
  }

  addTask() {
    this.newTask.listId = this.listId
    this.newTask.creatorId = this.authService.userData!.userData.id
    this.newTask.statusId = this.statusId

    this.taskService.post(this.newTask, this.spaceId).subscribe(() => this.taskAdded.emit())
    this.newTask = {
      title: '',
      description: '',
      creatorId: 0,
      listId: 0,
      statusId: this.statusId
    }
    this.isAddingTask = false
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.newTask.dueTimestamp = date.getTime()
  }
}
