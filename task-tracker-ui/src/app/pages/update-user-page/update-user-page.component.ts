import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import ChangeUserPasswordRequest from 'src/app/model/user/ChangeUserPasswordRequest';
import UpdateUserRequest from 'src/app/model/user/UpdateUserRequest';
import UserModel from 'src/app/model/user/UserModel';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-user-page',
  templateUrl: './update-user-page.component.html',
  styleUrls: ['./update-user-page.component.css']
})
export class UpdateUserPageComponent implements OnInit {

  user: UserModel

  changePasswordRequest: ChangeUserPasswordRequest = {
    userId: 0,
    currentPassword: '',
    newPassword: ''
  }

  confirmNewPassword: string = ''

  updateUserRequest: UpdateUserRequest = {
    id: 0
  }

  constructor(private authService: AuthService, private userService: UserService, private router: Router) {
    this.user = authService.userData!.userData
  }
  ngOnInit(): void {
    this.refreshRequest()
  }

  refreshRequest() {
    const { firstName, lastName, id } = this.user

    this.updateUserRequest = { firstName, lastName, id }
  }

  confirmPasswordUpdate() {
    const isConfirmed = confirm("Do you want to change your password? You will be logged out from all devices")

    if(!isConfirmed){
      return
    }

    this.changePasswordRequest.userId = this.user.id

    this.authService.changeUserPassword(this.changePasswordRequest).subscribe({
      error: (err) => alert(err.error.error),
      next: () => this.router.navigate(['/login'])
    })
  }

  confirmUserUpdate() {
    this.userService.update(this.updateUserRequest).subscribe(() => {
      this.userService.getById(this.user.id).subscribe(res => {
        this.user = res
        this.authService.userData!.userData = res

        alert('Profile updated!')
        this.refreshRequest()
      })
    })
  }
}
