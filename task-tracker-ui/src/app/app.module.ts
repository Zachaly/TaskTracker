import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { RouteGuard } from './route.guard';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { FormErrorListComponent } from './components/form-error-list/form-error-list.component';

const route = (path: string, component: any, canActivate: any[] = []) => ({ path, component, canActivate })

const routes: Routes = [
  route('register', RegisterPageComponent),
  route('login', LoginPageComponent),
  route('', MainPageComponent, [RouteGuard])
]

@NgModule({
  declarations: [
    AppComponent,
    RegisterPageComponent,
    LoginPageComponent,
    MainPageComponent,
    FormErrorListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [RouteGuard, AuthService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
