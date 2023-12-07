import { Component, OnInit } from '@angular/core';
import TaskListModel from 'src/app/model/task-list/TaskListModel';
import UserModel from 'src/app/model/user/UserModel';
import UpdateUserTaskRequest from 'src/app/model/user-task/UpdateUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

  public userData: UserModel
  public lists: TaskListModel[] = []

  constructor(private authService: AuthService, private taskService: UserTaskService, private taskListService: TaskListService,
    private taskStatusGroupService: TaskStatusGroupService) {
    this.userData = authService.userData!.userData!
  }

  ngOnInit(): void {
    this.taskListService.get({ creatorId: this.userData.id, joinStatusGroup: true })
      .subscribe(res => {
        this.lists = res
      })
  }

  public loadListTasks(list: TaskListModel) {
    this.taskStatusGroupService.getById(list.statusGroupId).subscribe(res => {
      list.statusGroup = res

      const maxIndex = Math.max(...res.statuses!.map(x => x.index))
      const closedStatus = res.statuses!.find(x => x.index == maxIndex)!

      this.taskService.get({ listId: list.id, skipStatusIds: [closedStatus.id] }).subscribe(res => {
        list.tasks = res.sort((a, b) => b.status.index - a.status.index)
      })
    })
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
