import { Component, Input } from '@angular/core';
import TaskListModel from 'src/app/model/TaskListModel';

@Component({
  selector: 'app-side-bar-list-link',
  templateUrl: './side-bar-list-link.component.html',
  styleUrls: ['./side-bar-list-link.component.css']
})
export class SideBarListLinkComponent {
  @Input() list: TaskListModel = {
    id: 0,
    title: '',
    creator: { id: 0, firstName: '', lastName: '', email: ''},
  }

  link = () => `/list/${this.list.id}`
}
