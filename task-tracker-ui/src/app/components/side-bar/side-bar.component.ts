import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import TaskListModel from 'src/app/model/TaskListModel';
import { AuthService } from 'src/app/services/auth.service';
import { TaskListService } from 'src/app/services/task-list.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  lists: TaskListModel[] = []

  constructor(private authService: AuthService, private router: Router, private taskListService: TaskListService) {

  }

  ngOnInit(): void {
    this.taskListService.get({ creatorId: this.authService.userData?.userData.id }).subscribe(res => this.lists = res)
  }

  public async logout() {
    this.authService.revokeToken()

    this.router.navigate(['/login'])
  }

  public isCurrentRoute(route: string){
    return this.router.url == route
  }
}
