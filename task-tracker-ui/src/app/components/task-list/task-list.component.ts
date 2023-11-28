import { Component, EventEmitter, Input, Output } from '@angular/core';
import TaskListModel from 'src/app/model/TaskListModel';
import UpdateUserTaskRequest from 'src/app/model/request/UpdateUserTaskRequest';
import { faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';

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
    statusGroupId: 0
  }
  @Input() userId = 0
 
  @Output() taskAdded: EventEmitter<any> = new EventEmitter()
  @Output() updateTask: EventEmitter<UpdateUserTaskRequest> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()
  @Output() loadTasks: EventEmitter<any> = new EventEmitter()

  showTasks = false

  curentIcon = faAngleDown

  toggleShowTasks(){
    this.showTasks = !this.showTasks

    if(this.showTasks){
      this.loadTasks.emit()
      this.curentIcon = faAngleUp
    } else {
      this.curentIcon = faAngleDown
    }
  }
}
