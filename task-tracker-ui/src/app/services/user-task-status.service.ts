import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import UserTaskStatusModel from '../model/user-task-status/UserTaskStatusModel';
import { GetUserTaskStatusRequest, mapGetUserTaskStatusRequest } from '../model/user-task-status/GetUserTaskStatusRequest';
import AddUserTaskStatusRequest from '../model/user-task-status/AddUserTaskStatusRequest';
import UpdateUserTaskRequest from '../model/user-task/UpdateUserTaskRequest';
import { CreatedResponseModel } from '../model/ResponseModel';

const API_URL = 'https://localhost:5001/api/user-task-status'

@Injectable({
  providedIn: 'root'
})
export class UserTaskStatusService {

  constructor(private http: HttpClient) { }

  get(request: GetUserTaskStatusRequest) {
    const params = mapGetUserTaskStatusRequest(request)

    return this.http.get<UserTaskStatusModel[]>(API_URL, { params })
  }

  getById(id: number) {
    return this.http.get<UserTaskStatusModel>(`${API_URL}/${id}`)
  }

  add(request: AddUserTaskStatusRequest) {
    return this.http.post<CreatedResponseModel>(API_URL, request)
  }

  update(request: UpdateUserTaskRequest) {
    return this.http.put(API_URL, request)
  }

  deleteById(id: number) {
    return this.http.delete(`${API_URL}/${id}`)
  }
}
