import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) {
    authService.userDataSubject.subscribe(res => {
      if(res && (router.url == '/register' || router.url == '/login')) {
        this.router.navigate(['/'])
      }
      if(!res) {
        this.router.navigate(['/login'])
      }
    })
  }
  ngOnInit(): void {
    this.authService.loadUserData()
  }
}
