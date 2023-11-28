import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import TaskStatusGroupModel from 'src/app/model/TaskStatusGroupModel';
import AddTaskListRequest from 'src/app/model/request/AddTaskListRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';
import { TaskStatusGroupService } from 'src/app/services/task-status-group.service';

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
    statusGroupId: 0
  }

  statusGroups: TaskStatusGroupModel[] = []

  constructor(private authService: AuthService, private taskListService: TaskListService, private router: Router,
    private taskStatusGroupService: TaskStatusGroupService) {

  }
  ngOnInit(): void {
    this.taskStatusGroupService.get({
      skipPagination: true,
      userId: this.authService.userData?.userData.id
    }).subscribe(res => this.statusGroups = res)
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
