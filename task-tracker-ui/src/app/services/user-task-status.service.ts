import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import UserTaskStatusModel from '../model/UserTaskStatusModel';
import { GetUserTaskStatusRequest, mapGetUserTaskStatusRequest } from '../model/request/get/GetUserTaskStatusRequest';
import AddUserTaskStatusRequest from '../model/request/AddUserTaskStatusRequest';
import UpdateUserTaskRequest from '../model/request/UpdateUserTaskRequest';
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
