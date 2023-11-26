import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import TaskStatusGroupModel from 'src/app/model/TaskStatusGroupModel';
import UserTaskStatusModel from 'src/app/model/UserTaskStatusModel';
import AddUserTaskStatusRequest from 'src/app/model/request/AddUserTaskStatusRequest';
import UpdateTaskStatusGroupRequest from 'src/app/model/request/UpdateTaskStatusGroupRequest';
import UpdateUserTaskStatusRequest from 'src/app/model/request/UpdateUserTaskStatusReqeust';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserTaskStatusService } from 'src/app/services/user-task-status.service';

@Component({
  selector: 'app-update-status-group-page',
  templateUrl: './update-status-group-page.component.html',
  styleUrls: ['./update-status-group-page.component.css']
})
export class UpdateStatusGroupPageComponent implements OnInit {

  group: TaskStatusGroupModel = {
    id: 0,
    name: '',
    isDefault: false,
    statuses: []
  }

  isUpdatingName = false
  newName = ''

  isAddingStatus = false

  newStatus: AddUserTaskStatusRequest = {
    name: '',
    color: '',
    index: 0,
    groupId: 0
  }

  constructor(private route: ActivatedRoute, private taskStatusGroupService: TaskStatusGroupService, private userTaskStatusService: UserTaskStatusService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['groupId']

      this.taskStatusGroupService.getById(id).subscribe(res => {
        this.group = res
        this.newName = res.name

        this.newStatus.groupId = res.id

        this.newStatus.index = Math.max(...res.statuses!.map(x => x.index).filter(x => x >= 0 && x < 20)) + 1
      })
    })
  }

  updateName() {
    const request: UpdateTaskStatusGroupRequest = {
      id: this.group.id,
      name: this.newName
    }

    this.taskStatusGroupService.update(request).subscribe(() => {
      this.taskStatusGroupService.getById(this.group.id).subscribe(res => {
        this.group.name = res.name
        this.isUpdatingName = false
      })
    })
  }

  addStatus() {
    this.userTaskStatusService.add(this.newStatus).subscribe(() => {
      this.reloadStatuses()
      this.isAddingStatus = false
    })
  }

  deleteStatus(id: number) {
    this.userTaskStatusService.deleteById(id)
      .subscribe(() => this.reloadStatuses())
  }

  reloadStatuses() {
    this.userTaskStatusService.get({ groupId: this.group.id, skipPagination: true }).subscribe(res => {
      this.group.statuses = res.sort((a, b) => a.index - b.index)
      this.newStatus.index = Math.max(...res.map(x => x.index).filter(x => x >= 0 && x < 20)) + 1
    })
  }

  moveStatusIndexUp(status: UserTaskStatusModel) {
    const next = this.group.statuses!.filter(x => x.index > status.index)
    if (next.length < 1 || status.index == Math.max(...this.group.statuses!.map(x => x.index).filter(x => x != 21 && x != 0))) {
      alert('Status hax max index')
      return
    }

    const updateRequest: UpdateUserTaskStatusRequest = {
      id: status.id,
      index: status.index + 1
    }

    this.userTaskStatusService.update(updateRequest).subscribe(() => this.reloadStatuses())
  }

  moveStatusIndexDown(status: UserTaskStatusModel) {
    const lesser = this.group.statuses!.filter(x => x.index < status.index)
    if (lesser.length < 1 || status.index == 1) {
      alert('Status has minimal index')
      return
    }

    const updateRequest: UpdateUserTaskStatusRequest = {
      id: status.id,
      index: status.index - 1
    }

    this.userTaskStatusService.update(updateRequest).subscribe(() => this.reloadStatuses())
  }

  updateStatus(request: UpdateUserTaskStatusRequest) {
    this.userTaskStatusService.update(request).subscribe(() => this.reloadStatuses())
  }
}
