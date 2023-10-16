import { Component } from '@angular/core';
import { Router } from '@angular/router';
import LoginRequest from 'src/app/model/request/LoginRequest';
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

  constructor(private router: Router, private authService: AuthService) {}

  public goToRegister(){
    this.router.navigate(['/register'])
  }

  public login() {
    this.authService.login(this.request).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => alert(err.error.error)
    })
  }
}
