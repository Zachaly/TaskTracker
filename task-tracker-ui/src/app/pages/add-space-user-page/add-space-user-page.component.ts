import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import AddSpaceUserRequest from 'src/app/model/space-user/AddSpaceUserRequest';
import GetUserRequest from 'src/app/model/user/GetUserRequest';
import UserModel from 'src/app/model/user/UserModel';
import { AuthService } from 'src/app/services/auth.service';
import { SpaceUserService } from 'src/app/services/space-user.service';
import { UserSpaceService } from 'src/app/services/user-space.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-space-user-page',
  templateUrl: './add-space-user-page.component.html',
  styleUrls: ['./add-space-user-page.component.css']
})
export class AddSpaceUserPageComponent implements OnInit {

  userId: number = 0
  spaceId: number = 0

  users: UserModel[] = []

  currentUserIds: number[] = []

  email: string = ''

  constructor(private route: ActivatedRoute, private spaceUserService: SpaceUserService,
    private userService: UserService, private authService: AuthService, private router: Router,
    private spaceService: UserSpaceService) {
      this.userId = authService.userData!!.userData.id

  }
  ngOnInit(): void {
    this.route.params.subscribe(res => {
      this.spaceId = res['spaceId']
      this.spaceUserService.get({ spaceId: this.spaceId, skipPagination: true})
        .subscribe(res => this.currentUserIds.push(...res.map(x => x.user.id), this.userId))
      this.spaceService.getById(this.spaceId).subscribe(res => this.currentUserIds.push(res.owner.id))
    })
  }

  public add(user: UserModel) {
    const request: AddSpaceUserRequest = {
      userId: user.id,
      spaceId: this.spaceId
    }

    this.spaceUserService.add(request).subscribe(() => {
      this.router.navigate(['space', this.spaceId.toString()])
    })
  }

  public searchUsers() {
    const request: GetUserRequest = {
      skipIds: this.currentUserIds,
      searchEmail: this.email
    }

    this.userService.get(request).subscribe(res => {
      this.users = res
    })
  }
}
