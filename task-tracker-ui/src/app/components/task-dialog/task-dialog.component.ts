import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import UserTaskModel from 'src/app/model/UserTaskModel';
import { UserTaskService } from 'src/app/services/user-task.service';

@Component({
  selector: 'app-task-dialog',
  templateUrl: './task-dialog.component.html',
  styleUrls: ['./task-dialog.component.css']
})
export class TaskDialogComponent implements OnInit {
  @Input() id: number = 0
  @Output() close: EventEmitter<any> = new EventEmitter()
  @Output() deleteTask: EventEmitter<number> = new EventEmitter()

  task: UserTaskModel = {
    id: 0,
    title: '',
    description: '',
    creationTimestamp: 0,
    creator: { id: 0, firstName: '', lastName: '', email: '' }
  }

  creationDate = () => new Date(this.task.creationTimestamp)
  dueDate = () => new Date(this.task.dueTimestamp!)

  constructor(private taskService: UserTaskService) {

  }
  ngOnInit(): void {
    this.taskService.getById(this.id).subscribe(res => this.task = res)
  }

  public onDeleteTask() {
    this.deleteTask.emit(this.task.id)
    this.close.emit()
  }
}
