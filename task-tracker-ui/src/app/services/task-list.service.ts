import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetTaskListRequest, { mapGetTaskListRequest } from '../model/task-list/GetTaskListRequest';
import { Observable } from 'rxjs';
import TaskListModel from '../model/task-list/TaskListModel';
import UpdateTaskListRequest from '../model/task-list/UpdateTaskListRequest';
import AddTaskListRequest from '../model/task-list/AddTaskListRequest';

const API_URL = 'https://localhost:5001/api/task-list'

@Injectable({
  providedIn: 'root'
})
export class TaskListService {

  constructor(private http: HttpClient) { }

  get(request: GetTaskListRequest): Observable<TaskListModel[]> {
    const params = mapGetTaskListRequest(request)
    return this.http.get<TaskListModel[]>(API_URL, { params })
  }

  getById(id: number): Observable<TaskListModel> {
    return this.http.get<TaskListModel>(`${API_URL}/${id}`)
  }

  add(request: AddTaskListRequest): Observable<any> {
    return this.http.post(API_URL, request, {
      headers: {
        'SpaceId': request.spaceId.toString()
      }
    })
  }

  update(request: UpdateTaskListRequest, spaceId: number): Observable<any> {
    return this.http.put(API_URL, request, {
      headers: {
        'SpaceId': spaceId.toString()
      }
    })
  }

  deleteById(id: number, spaceId: number): Observable<any> {
    return this.http.delete(`${API_URL}/${id}`, {
      headers: {
        'SpaceId': spaceId.toString()
      }
    })
  }
}
