import { Component } from '@angular/core';
import { Router } from '@angular/router';
import AddTaskListRequest from 'src/app/model/request/AddTaskListRequest';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';

@Component({
  selector: 'app-add-list-page',
  templateUrl: './add-list-page.component.html',
  styleUrls: ['./add-list-page.component.css']
})
export class AddListPageComponent {

  newList: AddTaskListRequest = {
    title: '',
    creatorId: 0,
    color: '',
    description: ''
  }

  constructor(private authService: AuthService, private taskListService: TaskListService, private router: Router) {

  }

  addList() {
    this.newList.creatorId = this.authService.userData!.userData.id
    console.log(this.newList)

    this.taskListService.add(this.newList).subscribe({
      next: () => this.router.navigate(['/']),
      error:(err) => console.log(err)
    })
  }
}
