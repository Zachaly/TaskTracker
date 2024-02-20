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

  public add(request: AddTaskAssignedUserRequest, spaceId: number) {
    return this.http.post(API_URL, request, {
      headers: {
        'SpaceId': spaceId.toString()
      }
    })
  }

  public delete(taskId: number, userId: number, spaceId: number) {
    return this.http.delete(`${API_URL}/${taskId}/${userId}`, {
      headers: {
        'SpaceId': spaceId.toString()
      }
    })
  }
}
