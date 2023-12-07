import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import TaskListModel from 'src/app/model/task-list/TaskListModel';
import TaskStatusGroupModel from 'src/app/model/task-status-group/TaskStatusGroupModel';
import UserTaskModel from 'src/app/model/user-task/UserTaskModel';
import AddUserTaskRequest from 'src/app/model/user-task/AddUserTaskRequest';
import UpdateTaskListRequest from 'src/app/model/task-list/UpdateTaskListRequest';
import UpdateUserTaskRequest from 'src/app/model/user-task/UpdateUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserTaskService } from 'src/app/services/user-task.service';
import GetUserTaskRequest from 'src/app/model/user-task/GetUserTaskRequest';
import { faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';

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

  titleOrderBy = 0
  creationDateOrderBy = 0
  dueDateOrderBy = 0
  priorityOrderBy = 0

  showClosed = false

  faAngleUp = faAngleUp
  faAngleDown = faAngleDown

  constructor(private route: ActivatedRoute, private taskListService: TaskListService, private taskService: UserTaskService,
    private authService: AuthService, private router: Router, private taskStatusGroupService: TaskStatusGroupService) {
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

        this.taskStatusGroupService.getById(res.statusGroupId)
          .subscribe(res => {
            this.statusGroup = res
            this.defaultStatusId = this.statusGroup.statuses!.find(x => x.index == 0)!.id

            this.loadTasks()
          })
      })
    })
  }

  loadTasks() {
    const getRequest: GetUserTaskRequest = {
      listId: this.list?.id
    }

    if (!this.showClosed) {
      const closedStatusIndex = Math.max(...this.statusGroup.statuses!.map(x => x.index))
      const closedStatus = this.statusGroup.statuses!.find(x => x.index == closedStatusIndex)!
      getRequest.skipStatusIds = [closedStatus.id]
    }

    this.taskService.get(getRequest).subscribe(res => {
      this.tasks = res.sort((a, b) => b.status.index - a.status.index)
    })
  }

  loadOrderedTasks(orderBy: string, isDescending: boolean = false) {
    const getRequest: GetUserTaskRequest = {
      listId: this.list?.id
    }

    if (isDescending) {
      getRequest.orderByDescending = orderBy
    } else {
      getRequest.orderBy = orderBy
    }

    if (!this.showClosed) {
      const closedStatusIndex = Math.max(...this.statusGroup.statuses!.map(x => x.index))
      const closedStatus = this.statusGroup.statuses!.find(x => x.index == closedStatusIndex)!
      getRequest.skipStatusIds = [closedStatus.id]
    }

    this.taskService.get(getRequest).subscribe(res => {
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

  updateTask(request: UpdateUserTaskRequest, task: UserTaskModel) {
    this.taskService.update(request).subscribe(() => {
      this.taskService.getById(request.id).subscribe(res => {
        task.priority = res.priority
        task.status = res.status
        task.title = res.title
        task.dueTimestamp = res.dueTimestamp
        task.description = res.description
      })
    })
  }

  orderTasksByTitle() {
    this.titleOrderBy++
    this.priorityOrderBy = 0
    this.creationDateOrderBy = 0
    this.dueDateOrderBy = 0

    if (this.titleOrderBy == 1) {
      this.loadOrderedTasks('Title', true)
    }
    else if (this.titleOrderBy == 2) {
      this.loadOrderedTasks('Title')
    }
    else {
      this.titleOrderBy = 0
      this.loadTasks()
    }
  }

  orderTasksByCreationDate() {
    this.priorityOrderBy = 0
    this.titleOrderBy = 0
    this.creationDateOrderBy++
    this.dueDateOrderBy = 0

    if (this.creationDateOrderBy == 1) {
      this.loadOrderedTasks('CreationTimestamp', true)
    }
    else if (this.creationDateOrderBy == 2) {
      this.loadOrderedTasks('CreationTimestamp')
    }
    else {
      this.creationDateOrderBy = 0
      this.loadTasks()
    }
  }

  orderTasksByDueDate() {
    this.priorityOrderBy = 0
    this.titleOrderBy = 0
    this.creationDateOrderBy = 0
    this.dueDateOrderBy++

    if (this.dueDateOrderBy == 1) {
      this.loadOrderedTasks('DueTimestamp', true)
    }
    else if (this.dueDateOrderBy == 2) {
      this.loadOrderedTasks('DueTimestamp')
    }
    else {
      this.dueDateOrderBy = 0
      this.loadTasks()
    }
  }

  orderTasksByPriority() {
    this.priorityOrderBy++
    this.titleOrderBy = 0
    this.creationDateOrderBy = 0
    this.dueDateOrderBy = 0

    if (this.priorityOrderBy == 1) {
      this.loadOrderedTasks('Priority', true)
    }
    else if (this.priorityOrderBy == 2) {
      this.loadOrderedTasks('Priority')
    }
    else {
      this.priorityOrderBy = 0
      this.loadTasks()
    }
  }

  toggleShowClosed() {
    this.showClosed = !this.showClosed
    this.loadTasks()
  }
}
