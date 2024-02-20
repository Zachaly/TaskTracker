import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

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
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskStatusPageComponent } from './pages/task-status-page/task-status-page.component';
import { UpdateStatusGroupPageComponent } from './pages/update-status-group-page/update-status-group-page.component';
import { TaskStatusListItemComponent } from './components/task-status-list-item/task-status-list-item.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { UpdateUserPageComponent } from './pages/update-user-page/update-user-page.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { AddSpacePageComponent } from './pages/add-space-page/add-space-page.component';
import { SpacePageComponent } from './pages/space-page/space-page.component';
import { SideBarSpaceLinkComponent } from './components/side-bar-space-link/side-bar-space-link.component';
import { DocumentPageComponent } from './pages/document-page/document-page.component';
import { AddSpaceUserPageComponent } from './pages/add-space-user-page/add-space-user-page.component';
import { ManageTaskAssignedUsersDialogComponent } from './components/manage-task-assigned-users-dialog/manage-task-assigned-users-dialog.component';
import { ManagePermissionsPageComponent } from './pages/manage-permissions-page/manage-permissions-page.component';

const route = (path: string, component: any, canActivate: any[] = []) => ({ path, component, canActivate })

const routes: Routes = [
  route('register', RegisterPageComponent),
  route('login', LoginPageComponent),
  route('', MainPageComponent, [RouteGuard]),
  route('list/add/:spaceId', AddListPageComponent, [RouteGuard]),
  route('list/:id', TaskListPageComponent, [RouteGuard]),
  route('task-status', TaskStatusPageComponent, [RouteGuard]),
  route('task-status/update/:groupId', UpdateStatusGroupPageComponent, [RouteGuard]),
  route('profile/update', UpdateUserPageComponent, [RouteGuard]),
  route('space/add', AddSpacePageComponent, [RouteGuard]),
  route('space/:id', SpacePageComponent, [RouteGuard]),
  route('doc/:id', DocumentPageComponent, [RouteGuard]),
  route('space/:spaceId/add-user', AddSpaceUserPageComponent, [RouteGuard]),
  route('space/:spaceId/permissions', ManagePermissionsPageComponent, [RouteGuard])
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
    TaskListComponent,
    TaskStatusPageComponent,
    UpdateStatusGroupPageComponent,
    TaskStatusListItemComponent,
    UpdateUserPageComponent,
    UserProfileComponent,
    AddSpacePageComponent,
    SpacePageComponent,
    SideBarSpaceLinkComponent,
    DocumentPageComponent,
    AddSpaceUserPageComponent,
    ManageTaskAssignedUsersDialogComponent,
    ManagePermissionsPageComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes),
    FontAwesomeModule,
    QuillModule.forRoot()
  ],
  providers: [RouteGuard, AuthService, UserService, tokenInterceptor],
  bootstrap: [AppComponent]
})
export class AppModule { }
