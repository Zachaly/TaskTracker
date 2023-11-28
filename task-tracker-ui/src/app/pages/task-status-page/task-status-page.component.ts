import { Component, OnInit } from '@angular/core';
import TaskStatusGroupModel from 'src/app/model/TaskStatusGroupModel';
import AddTaskStatusGroupRequest from 'src/app/model/request/AddTaskStatusGroupRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserTaskStatusService } from 'src/app/services/user-task-status.service';

@Component({
  selector: 'app-task-status-page',
  templateUrl: './task-status-page.component.html',
  styleUrls: ['./task-status-page.component.css']
})
export class TaskStatusPageComponent implements OnInit {

  groups: TaskStatusGroupModel[] = []
  userId: number = 0
  newGroup: AddTaskStatusGroupRequest = {
    userId: 0,
    name: ''
  }

  isAddingGroup = false

  constructor(private taskStatusGroupService: TaskStatusGroupService, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.userId = this.authService.userData!.userData.id
    this.loadGroups()
  }

  loadGroups() {
    this.taskStatusGroupService.get({ userId: this.userId }).subscribe(res => this.groups = res)
  }

  addGroup() {
    this.newGroup.userId = this.userId

    this.taskStatusGroupService.add(this.newGroup).subscribe(res => {
      this.isAddingGroup = false
      this.newGroup = {
        userId: this.userId,
        name: ''
      }
      this.taskStatusGroupService.getById(res.newEntityId!).subscribe(res => this.groups.push(res))
    })
  }

  deleteGroup(id: number) {
    this.taskStatusGroupService.deleteById(id).subscribe(() => this.groups = this.groups.filter(x => x.id !== id))
  }
}
