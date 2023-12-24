import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import UserSpaceModel from 'src/app/model/user-space/UserSpaceModel';
import UpdateUserSpaceRequest from 'src/app/model/user-space/request/UpdateUserSpaceRequest';
import { UserSpaceService } from 'src/app/services/user-space.service';

@Component({
  selector: 'app-space-page',
  templateUrl: './space-page.component.html',
  styleUrls: ['./space-page.component.css']
})
export class SpacePageComponent implements OnInit {

  spaceId: number = 0

  space: UserSpaceModel = {
    id: 0,
    owner: {
      id: 0,
      firstName: '',
      lastName: '',
      email: ''
    },
    title: '',
    statusGroup: {
      id: 0,
      statuses: [],
      name: '',
      isDefault: false
    },
    lists: []
  }

  updateRequest: UpdateUserSpaceRequest = {
    id: 0,
    title: ''
  }

  isUpdatingSpace = false

  constructor(private spaceService: UserSpaceService, private route: ActivatedRoute, private router: Router) {

  } 

  ngOnInit(): void {
    this.route.params.subscribe(param => {
      this.spaceId = param['id']
      this.loadSpace()
    })
  }

  private loadSpace() {
    this.spaceService.getById(this.spaceId).subscribe(res => {
      this.space = res
      this.updateRequest.id = res.id
      this.updateRequest.title = res.title
    })
  }

  update() {
    this.spaceService.update(this.updateRequest).subscribe(() => {
      this.loadSpace()
      this.isUpdatingSpace = false
    })
  }

  deleteSpace() {
    this.spaceService.deleteById(this.spaceId).subscribe(() => {
      this.router.navigate(['/'])
    })
  }
}
