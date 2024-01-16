import { Component, EventEmitter, Input, Output } from '@angular/core';
import TaskListModel from 'src/app/model/task-list/TaskListModel';
import UpdateUserTaskRequest from 'src/app/model/user-task/UpdateUserTaskRequest';
import { faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';
import UserTaskModel from 'src/app/model/user-task/UserTaskModel';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  @Input() list: TaskListModel = {
    id: 0,
    title: '',
    description: '',
    creator: { id: 0, lastName: '', firstName: '', email: '' },
    statusGroupId: 0,
    spaceId: 0
  }
  @Input() userId = 0

  @Output() taskAdded: EventEmitter<any> = new EventEmitter()
  @Output() updateTask: EventEmitter<UpdateUserTaskRequest> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() loadTasks: EventEmitter<any> = new EventEmitter()

  showTasks = false

  curentIcon = faAngleDown

  defaultStatus = () => this.list.statusGroup?.statuses?.sort((a, b) => a.index - b.index)[0]

  toggleShowTasks() {
    this.showTasks = !this.showTasks

    if (this.showTasks) {
      this.loadTasks.emit()
      this.curentIcon = faAngleUp
    } else {
      this.curentIcon = faAngleDown
    }
  }

  onTaskUpdated(task: UserTaskModel) {
    const taskToUpdate = this.list.tasks?.find(x => x.id == task.id)!

    taskToUpdate.title = task.title
    taskToUpdate.description = task.description
    taskToUpdate.priority = task.priority
    taskToUpdate.status = task.status
  }
}
