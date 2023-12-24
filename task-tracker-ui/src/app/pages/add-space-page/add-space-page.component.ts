import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import TaskStatusGroupModel from 'src/app/model/task-status-group/TaskStatusGroupModel';
import AddUserSpaceRequest from 'src/app/model/user-space/request/AddUserSpaceRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserSpaceService } from 'src/app/services/user-space.service';

@Component({
  selector: 'app-add-space-page',
  templateUrl: './add-space-page.component.html',
  styleUrls: ['./add-space-page.component.css']
})
export class AddSpacePageComponent implements OnInit {

  addRequest: AddUserSpaceRequest = {
    userId: 0,
    statusGroupId: 0,
    title: ''
  }

  userId = 0

  statusGroups: TaskStatusGroupModel[] = []

  constructor(private userSpaceService: UserSpaceService, private authService: AuthService, private statusGroupService: TaskStatusGroupService,
    private router: Router) {
    this.userId = authService.userData!.userData.id
  }

  ngOnInit(): void {
    this.statusGroupService.get({ userId: this.userId, skipPagination: true }).subscribe(res => {
      this.statusGroups = res
    })
  }

  addSpace() {
    this.addRequest.userId = this.userId

    this.userSpaceService.add(this.addRequest).subscribe(res => {
      this.router.navigate(['/space', res.newEntityId])
    })
  }
}
