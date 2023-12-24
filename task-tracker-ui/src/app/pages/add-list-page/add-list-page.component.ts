import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import TaskStatusGroupModel from 'src/app/model/task-status-group/TaskStatusGroupModel';
import AddTaskListRequest from 'src/app/model/task-list/AddTaskListRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';
import { UserSpaceService } from 'src/app/services/user-space.service';

@Component({
  selector: 'app-add-list-page',
  templateUrl: './add-list-page.component.html',
  styleUrls: ['./add-list-page.component.css']
})
export class AddListPageComponent implements OnInit {

  newList: AddTaskListRequest = {
    title: '',
    creatorId: 0,
    color: '',
    description: '',
    statusGroupId: 0,
    spaceId: 0
  }

  statusGroups: TaskStatusGroupModel[] = []

  constructor(private authService: AuthService, private taskListService: TaskListService, private router: Router,
    private taskStatusGroupService: TaskStatusGroupService, private route: ActivatedRoute, private spaceService: UserSpaceService) {

  }
  ngOnInit(): void {
    this.taskStatusGroupService.get({
      skipPagination: true,
      userId: this.authService.userData?.userData.id
    }).subscribe(res => this.statusGroups = res)
    this.route.params.subscribe(param => {
      this.newList.spaceId = param['spaceId']
      this.spaceService.getById(param['spaceId']).subscribe(res => this.newList.statusGroupId = res.statusGroup.id)
    })
  }

  addList() {
    this.newList.creatorId = this.authService.userData!.userData.id
    console.log(this.newList)

    this.taskListService.add(this.newList).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => console.log(err)
    })
  }
}
