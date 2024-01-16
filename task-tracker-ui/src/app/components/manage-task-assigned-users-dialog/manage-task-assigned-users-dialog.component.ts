import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import AddTaskAssignedUserRequest from 'src/app/model/task-assigned-user/AddTaskAssignedUserRequest';
import GetUserRequest from 'src/app/model/user/GetUserRequest';
import UserModel from 'src/app/model/user/UserModel';
import { SpaceUserService } from 'src/app/services/space-user.service';
import { TaskAssignedUserService } from 'src/app/services/task-assigned-user.service';
import { UserSpaceService } from 'src/app/services/user-space.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-manage-task-assigned-users-dialog',
  templateUrl: './manage-task-assigned-users-dialog.component.html',
  styleUrls: ['./manage-task-assigned-users-dialog.component.css']
})
export class ManageTaskAssignedUsersDialogComponent implements OnInit {
  @Input() taskId: number = 0
  @Input() spaceId: number = 0
  @Output() userDeleted: EventEmitter<number> = new EventEmitter()
  @Output() userAdded: EventEmitter<UserModel> = new EventEmitter()
  @Output() close: EventEmitter<any> = new EventEmitter()

  assignedUsers: UserModel[] = []

  searchedUsers: UserModel[] = []

  searchEmail: string = ''

  spaceUserIds: number[] = []

  constructor(private taskAssignedUserService: TaskAssignedUserService, private userService: UserService, private spaceUserService: SpaceUserService, 
    private userSpaceService: UserSpaceService) {

  }

  ngOnInit(): void {
    this.taskAssignedUserService.get({ taskId: this.taskId }).subscribe(res => {
      this.assignedUsers = res.map(x => x.user)
    })

    this.userSpaceService.getById(this.spaceId).subscribe(res => this.spaceUserIds.push(res.owner.id))

    this.spaceUserService.get({ spaceId: this.spaceId, skipPagination: true }).subscribe(res => this.spaceUserIds.push(...res.map(x => x.user.id)))
  }

  deleteUser(userId: number) {
    this.taskAssignedUserService.delete(this.taskId, userId).subscribe(() => {
      this.assignedUsers = this.assignedUsers.filter(x => x.id !== userId)
      this.userDeleted.emit(userId)
    })
  }

  addUser(user: UserModel) {
    const request: AddTaskAssignedUserRequest = {
      userId: user.id,
      taskId: this.taskId
    }

    this.taskAssignedUserService.add(request).subscribe(() => {
      this.assignedUsers.push(user)
      this.userAdded.emit(user)
      this.searchedUsers = this.searchedUsers.filter(u => u.id !== user.id)
    })
  }

  searchUsers() {
    const request: GetUserRequest = {
      searchEmail: this.searchEmail,
      skipIds: this.assignedUsers.map(x => x.id),
      ids: this.spaceUserIds
    }

    this.userService.get(request).subscribe(res => this.searchedUsers = res)
  }
}
