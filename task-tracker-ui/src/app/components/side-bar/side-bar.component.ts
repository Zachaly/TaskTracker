import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent {
  constructor(private authService: AuthService, private router: Router) {

  }

  public async logout() {
    this.authService.revokeToken()

    this.router.navigate(['/login'])
  }

  public isCurrentRoute(route: string){
    return this.router.url == route
  }
}
