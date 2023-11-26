import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetTaskStatusGroupRequest, mapGetTaskStatusGroupRequest } from '../model/request/get/GetUserTaskStatusGroupRequest';
import AddTaskStatusGroupRequest from '../model/request/AddTaskStatusGroupRequest';
import UpdateTaskStatusGroupRequest from '../model/request/UpdateTaskStatusGroupRequest';
import TaskStatusGroupModel from '../model/TaskStatusGroupModel';
import { CreatedResponseModel } from '../model/ResponseModel';

const API_URL = 'https://localhost:5001/api/task-status-group'

@Injectable({
  providedIn: 'root'
})
export class TaskStatusGroupService {

  constructor(private http: HttpClient) { }

  get(request: GetTaskStatusGroupRequest) {
    const params = mapGetTaskStatusGroupRequest(request)

    return this.http.get<TaskStatusGroupModel[]>(API_URL, { params })
  }

  getById(id: number) {
    return this.http.get<TaskStatusGroupModel>(`${API_URL}/${id}`)
  }

  add(request: AddTaskStatusGroupRequest) {
    return this.http.post<CreatedResponseModel>(API_URL, request)
  }

  update(request: UpdateTaskStatusGroupRequest) {
    return this.http.put(API_URL, request)
  }

  deleteById(id: number) {
    return this.http.delete(`${API_URL}/${id}`)
  }
}
