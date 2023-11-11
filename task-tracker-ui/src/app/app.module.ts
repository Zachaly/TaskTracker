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
import { TaskListPageComponent } from './pages/task-list-page/task-list-page.component';
import { AddTaskFormComponent } from './components/add-task-form/add-task-form.component';
import { SideBarListLinkComponent } from './components/side-bar-list-link/side-bar-list-link.component';
import { AddListPageComponent } from './pages/add-list-page/add-list-page.component';

const route = (path: string, component: any, canActivate: any[] = []) => ({ path, component, canActivate })

const routes: Routes = [
  route('register', RegisterPageComponent),
  route('login', LoginPageComponent),
  route('', MainPageComponent, [RouteGuard]),
  route('list/add', AddListPageComponent, [RouteGuard]),
  route('list/:id', TaskListPageComponent, [RouteGuard])
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
    TaskDialogComponent,
    TaskListPageComponent,
    AddTaskFormComponent,
    SideBarListLinkComponent,
    AddListPageComponent,
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
