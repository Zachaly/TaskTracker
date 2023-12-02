import { Component } from '@angular/core';
import { Router } from '@angular/router';
import ResponseModel from 'src/app/model/ResponseModel';
import RegisterRequest from 'src/app/model/user/RegisterRequest';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {

  public request: RegisterRequest = {
    email: '',
    firstName: '',
    lastName: '',
    password: ''
  }

  public validationErrors: {
    Password?: string[],
    FirstName?: string[],
    LastName?: string[],
    Email?: string[]
  } = {}

  constructor(private router: Router, private userService: UserService) { }

  public goToLogin() {
    this.router.navigate(['/login'])
  }

  public register() {
    this.userService.register(this.request).subscribe({
      next: () => {
        alert('Account created')
        this.router.navigate(['/login'])
      },
      error: (err) => {
        alert(err.error.error)
        if (err.error.validationErrors) {
          this.validationErrors = err.error.validationErrors
        }
      }
    })
  }
}
