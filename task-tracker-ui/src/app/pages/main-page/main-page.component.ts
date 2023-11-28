import { Component, OnInit } from '@angular/core';
import TaskListModel from 'src/app/model/TaskListModel';
import UserModel from 'src/app/model/UserModel';
import UserTaskModel from 'src/app/model/UserTaskModel';
import AddUserTaskRequest from 'src/app/model/request/AddUserTaskRequest';
import UpdateUserTaskRequest from 'src/app/model/request/UpdateUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

  public userData: UserModel
  public lists: TaskListModel[] = []

  constructor(private authService: AuthService, private taskService: UserTaskService, private taskListService: TaskListService) {
    this.userData = authService.userData!.userData!
  }

  ngOnInit(): void {
    this.taskListService.get({ creatorId: this.userData.id, joinStatusGroup: true })
      .subscribe(res => this.lists = res)
  }

  public loadListTasks(list: TaskListModel, reload: boolean = false) {
    if (list.tasks && !reload) {
      return
    }

    this.taskService.get({ listId: list.id }).subscribe(res => list.tasks = res)
  }

  deleteTask(id: number, list: TaskListModel) {
    this.taskService.deleteById(id).subscribe(() => {
      list.tasks = list.tasks?.filter(x => x.id !== id)
    })
  }

  updateTask(request: UpdateUserTaskRequest, list: TaskListModel) {
    this.taskService.update(request).subscribe(() => {
      this.taskService.getById(request.id).subscribe(updatedTask => {
        list.tasks![list.tasks!.findIndex(t => t.id == updatedTask.id)] = updatedTask
      })
    })
  }
}
