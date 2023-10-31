import { Component, OnInit } from '@angular/core';
import UserModel from 'src/app/model/UserModel';
import UserTaskModel from 'src/app/model/UserTaskModel';
import AddUserTaskRequest from 'src/app/model/request/get/AddUserTaskRequest';
import { AuthService } from 'src/app/services/auth.service';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

  public userData: UserModel
  public tasks: UserTaskModel[] = []
  public isAddingTask = false
  public newTask: AddUserTaskRequest = {
    title: '',
    creatorId: 0,
    description: ''
  }

  constructor(private authService: AuthService, private taskService: UserTaskService) { 
    this.userData = authService.userData!.userData!
  }

  ngOnInit(): void {
    this.taskService.get({ creatorId: this.userData.id }).subscribe(res => this.tasks = res)
  }

  private loadTasks() {
    this.taskService.get({ creatorId: this.userData.id }).subscribe(res => this.tasks = res)
  }

  deleteTask(id: number) {
    this.taskService.deleteById(id).subscribe(() => {
      this.tasks = this.tasks.filter(t => t.id !== id)
    })    
  }

  cancelAddingTask() {
    this.newTask = {
      title: '',
      description: '',
      creatorId: this.userData.id
    }
    this.isAddingTask = false
  }

  addTask() {
    this.newTask.creatorId = this.userData.id
    this.taskService.post(this.newTask).subscribe(() => this.loadTasks())
    this.newTask = {
      title: '',
      description: '',
      creatorId: this.userData.id
    }
    this.isAddingTask = false
  }

  changeDueDate(e: Event) {
    const target = e.target as HTMLInputElement

    var date = new Date(target.value)

    this.newTask.dueTimestamp = date.getTime()
  }
}
