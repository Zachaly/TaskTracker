import { Component } from '@angular/core';
import UserModel from 'src/app/model/UserModel';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  public data: UserModel

  constructor(private authService: AuthService) { 
    this.data = authService.userData!.userData!
  }
}
