import { Component, EventEmitter, Input, Output } from '@angular/core';
import TaskListModel from 'src/app/model/TaskListModel';
import AddUserTaskRequest from 'src/app/model/request/AddUserTaskRequest';
import UpdateUserTaskRequest from 'src/app/model/request/UpdateUserTaskRequest';

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
    creator: { id: 0, lastName: '', firstName: '', email: '' }
  }
  @Input() userId = 0
 
  @Output() taskAdded: EventEmitter<any> = new EventEmitter()
  @Output() updateTask: EventEmitter<UpdateUserTaskRequest> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() loadTasks: EventEmitter<any> = new EventEmitter()

  showTasks = false

  toggleShowTasks(){
    this.showTasks = !this.showTasks

    if(this.showTasks){
      this.loadTasks.emit()
    }
  }
}
