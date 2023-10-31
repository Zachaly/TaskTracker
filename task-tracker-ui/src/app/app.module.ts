import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { RouteGuard } from './infrastructure/route.guard';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { FormErrorListComponent } from './components/form-error-list/form-error-list.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { SideBarLinkComponent } from './components/side-bar-link/side-bar-link.component';
import { UserTaskListItemComponent } from './components/user-task-list-item/user-task-list-item.component';
import { TaskDialogComponent } from './components/task-dialog/task-dialog.component';
import { tokenInterceptor } from './infrastructure/token-refresh-interceptor';

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
    FormErrorListComponent,
    SideBarComponent,
    SideBarLinkComponent,
    UserTaskListItemComponent,
    TaskDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [RouteGuard, AuthService, UserService, tokenInterceptor],
  bootstrap: [AppComponent]
})
export class AppModule { }
