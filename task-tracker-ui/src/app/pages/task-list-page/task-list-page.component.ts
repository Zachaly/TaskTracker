import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import TaskListModel from 'src/app/model/TaskListModel';
import UserTaskModel from 'src/app/model/UserTaskModel';
import AddUserTaskRequest from 'src/app/model/request/AddUserTaskRequest';
import UpdateTaskListRequest from 'src/app/model/request/UpdateTaskListRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
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
    description: ''
  }

  listId: number = 0
  userId: number = 0
  isUpdatingList = false

  updateRequest: UpdateTaskListRequest = {
    id: 0,
  }

  constructor(private route: ActivatedRoute, private taskListService: TaskListService, private taskService: UserTaskService,
    private authService: AuthService, private router: Router) {
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

    if(!shouldDelete) {
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
}
