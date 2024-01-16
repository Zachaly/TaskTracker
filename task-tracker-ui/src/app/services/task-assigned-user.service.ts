import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetTaskAssignedUserRequest, { mapGetTaskAssignedUserRequest } from '../model/task-assigned-user/GetTaskAssignedUserRequest';
import TaskAssignedUserModel from '../model/task-assigned-user/TaskAssignedUserModel';
import AddTaskAssignedUserRequest from '../model/task-assigned-user/AddTaskAssignedUserRequest';

const API_URL = 'https://localhost:5001/api/task-assigned-user'

@Injectable({
  providedIn: 'root'
})
export class TaskAssignedUserService {

  constructor(private http: HttpClient) { }

  public get(request: GetTaskAssignedUserRequest) {
    const params = mapGetTaskAssignedUserRequest(request)

    return this.http.get<TaskAssignedUserModel[]>(API_URL, { params })
  }

  public add(request: AddTaskAssignedUserRequest) {
    return this.http.post(API_URL, request)
  }

  public delete(taskId: number, userId: number) {
    return this.http.delete(`${API_URL}/${taskId}/${userId}`)
  }
}
