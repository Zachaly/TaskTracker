import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import AddSpaceUserPermissionsRequest from 'src/app/model/space-user-permissions/AddSpaceUserPermissionsRequest';
import SpaceUserPermissionsModel from 'src/app/model/space-user-permissions/SpaceUserPermissionsModel';
import UpdateSpaceUserPermissionsRequest from 'src/app/model/space-user-permissions/UpdateSpaceUserPermissionsRequest';
import SpaceUserModel from 'src/app/model/space-user/SpaceUserModel';
import { SpaceUserPermissionsService } from 'src/app/services/space-user-permissions.service';
import { SpaceUserService } from 'src/app/services/space-user.service';

@Component({
  selector: 'app-manage-permissions-page',
  templateUrl: './manage-permissions-page.component.html',
  styleUrls: ['./manage-permissions-page.component.css']
})
export class ManagePermissionsPageComponent implements OnInit {

  permissions: SpaceUserPermissionsModel[] = []
  spaceId: number = 0
  searchedUsers: SpaceUserModel[] = []
  selectedUserId?: number
  isAddingUser = false

  constructor(private spaceUserPermissionsService: SpaceUserPermissionsService, private route: ActivatedRoute,
    private spaceUserService: SpaceUserService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.spaceId = params['spaceId']

      this.spaceUserPermissionsService.get({ spaceId: this.spaceId }).subscribe(res => this.permissions = res)
    })
  }

  updatePermissions(permissions: SpaceUserPermissionsModel) {
    const request: UpdateSpaceUserPermissionsRequest = {
      userId: permissions.user.id,
      spaceId: permissions.spaceId,
      canAddUsers: permissions.canAddUsers,
      canAssignTaskUsers: permissions.canAssignTaskUsers,
      canChangePermissions: permissions.canChangePermissions,
      canModifyLists: permissions.canModifyLists,
      canModifySpace: permissions.canModifySpace,
      canModifyTasks: permissions.canModifyTasks,
      canRemoveLists: permissions.canRemoveLists,
      canRemoveTasks: permissions.canRemoveTasks,
      canRemoveUsers: permissions.canRemoveUsers
    }

    this.spaceUserPermissionsService.update(request).subscribe(res => {
      this.spaceUserPermissionsService.get({ spaceId: request.spaceId, userId: request.userId }).subscribe(res => {
        this.spaceUserPermissionsService.get({ spaceId: this.spaceId }).subscribe(res => this.permissions = res)
      })
    })
  }

  selectUser() {
    if (!this.selectedUserId) {
      return
    }

    console.log(this.selectedUserId)

    this.spaceUserPermissionsService.add({ userId: this.selectedUserId, spaceId: this.spaceId })
      .subscribe(() =>
        this.spaceUserPermissionsService.get({ spaceId: this.spaceId })
          .subscribe(res => this.permissions = res)
      )
    this.isAddingUser = false
  }

  toggleIsAddingUser() {
    this.isAddingUser = !this.isAddingUser

    if (this.isAddingUser) {
      this.spaceUserService.get({ spaceId: this.spaceId, skipUserIds: this.permissions.map(x => x.user.id) })
        .subscribe(res => this.searchedUsers = res)
    }
  }
}
