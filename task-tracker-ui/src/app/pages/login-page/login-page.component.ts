import { Component } from '@angular/core';
import { Router } from '@angular/router';
import LoginRequest from 'src/app/model/user/LoginRequest';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {

  public request: LoginRequest = {
    email: '',
    password: ''
  }

  public rememberMe = false

  constructor(private router: Router, private authService: AuthService) { }

  public goToRegister() {
    this.router.navigate(['/register'])
  }

  public login() {
    this.authService.login(this.request, this.rememberMe).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => alert(err.error.error)
    })
  }
}
