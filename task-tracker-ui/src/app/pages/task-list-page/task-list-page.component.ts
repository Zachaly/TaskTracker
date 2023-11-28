import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import TaskListModel from 'src/app/model/TaskListModel';
import TaskStatusGroupModel from 'src/app/model/TaskStatusGroupModel';
import UserTaskModel from 'src/app/model/UserTaskModel';
import AddUserTaskRequest from 'src/app/model/request/AddUserTaskRequest';
import UpdateTaskListRequest from 'src/app/model/request/UpdateTaskListRequest';
import UpdateUserTaskRequest from 'src/app/model/request/UpdateUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserTaskStatusService } from 'src/app/services/user-task-status.service';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-task-list-page',
  templateUrl: './task-list-page.component.html',
  styleUrls: ['./task-list-page.component.css']
})
export class TaskListPageComponent implements OnInit {
  list?: TaskListModel
  tasks: UserTaskModel[] = []
  newTask: AddUserTaskRequest = {
    title: '',
    creatorId: 0,
    listId: 0,
    description: '',
    statusId: 0
  }

  listId: number = 0
  userId: number = 0
  isUpdatingList = false

  statusGroup: TaskStatusGroupModel = {
    id: 0,
    statuses: [],
    name: '',
    isDefault: false
  }

  defaultStatusId = 0

  updateRequest: UpdateTaskListRequest = {
    id: 0,
  }

  constructor(private route: ActivatedRoute, private taskListService: TaskListService, private taskService: UserTaskService,
    private authService: AuthService, private router: Router, private taskStatusGroupService: TaskStatusGroupService,
    private taskStatusService: UserTaskStatusService) {
    this.userId = authService.userData!.userData.id
  }

  ngOnInit(): void {
    this.route.params.subscribe(param => {
      const id = param['id']
      this.listId = id

      this.taskListService.getById(id).subscribe(res => {
        this.list = res
        this.updateRequest = {
          id: res.id,
          title: res.title,
          color: res.color,
          description: res.description
        }
        this.taskService.get({ listId: id }).subscribe(res => {
          this.tasks = res
        })

        this.taskStatusGroupService.getById(res.statusGroupId)
          .subscribe(res => {
            this.statusGroup = res
            this.defaultStatusId = this.statusGroup.statuses!.find(x => x.index == 0)!.id
          })
      })
    })
  }

  loadTasks() {
    this.taskService.get({ listId: this.listId }).subscribe(res => {
      this.tasks = res
    })
  }

  deleteTask(id: number) {
    this.taskService.deleteById(id).subscribe(() => {
      this.loadTasks()
    })
  }

  deleteList() {
    const shouldDelete = confirm('Do you want to delete this list?')

    if (!shouldDelete) {
      return
    }

    this.taskListService.deleteById(this.list!.id).subscribe(() => this.router.navigate(['/']))
  }

  updateList() {
    this.taskListService.update(this.updateRequest).subscribe(() => {
      this.isUpdatingList = false
      this.taskListService.getById(this.list!.id).subscribe(res => this.list = res)
    })
  }

  updateTaskTitle(id: number, title: string) {
    const request: UpdateUserTaskRequest = {
      id,
      title
    }

    this.taskService.update(request).subscribe(() => {
      this.tasks.find(x => x.id == id)!.title = title
    })
  }

  updateTaskDueTimestamp(id: number, dueTimestamp?: number) {
    const request: UpdateUserTaskRequest = {
      id,
      dueTimestamp
    }

    this.taskService.update(request).subscribe(() => {
      this.tasks.find(x => x.id == id)!.dueTimestamp = dueTimestamp
    })
  }

  updateTaskStatus(id: number, statusId: number){
    const request: UpdateUserTaskRequest = {
      id,
      statusId
    }

    this.taskService.update(request).subscribe(() => {
      this.taskService.getById(request.id).subscribe(res => this.tasks.find(x => x.id == res.id)!.status = res.status)
    })
  }

  taskUpdated(task: UserTaskModel) {
    console.log(task)
    this.tasks[this.tasks.findIndex(x => x.id == task.id)]! = task
  }
}
