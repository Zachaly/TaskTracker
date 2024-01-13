import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import UserSpaceModel from 'src/app/model/user-space/UserSpaceModel';
import { AuthService } from 'src/app/services/auth.service';
import { SpaceUserService } from 'src/app/services/space-user.service';
import { UserSpaceService } from 'src/app/services/user-space.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  spaces: UserSpaceModel[] = []

  constructor(private authService: AuthService, private router: Router, private spaceService: UserSpaceService,
    private spaceUserService: SpaceUserService) {

  }

  ngOnInit(): void {
    this.spaceService.get({ ownerId: this.authService.userData?.userData.id }).subscribe(res => this.spaces.push(...res))
    this.spaceUserService.get({ userId: this.authService.userData!.userData.id }).subscribe(res => this.spaces.push(...res.map(x => x.space)))
  }

  public async logout() {
    this.authService.revokeToken()

    this.router.navigate(['/login'])
  }

  public isCurrentRoute(route: string) {
    return this.router.url == route
  }
}
