import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import GetUserTaskRequest, { mapGetUserTaskRequest } from '../model/user-task/GetUserTaskRequest';
import { Observable } from 'rxjs';
import UserTaskModel from '../model/user-task/UserTaskModel';
import AddUserTaskRequest from '../model/user-task/AddUserTaskRequest';
import UpdateUserTaskRequest from '../model/user-task/UpdateUserTaskRequest';

const API_URL = 'https://localhost:5001/api/user-task'

@Injectable({
  providedIn: 'root'
})
export class UserTaskService {

  constructor(private http: HttpClient) { }

  public get(request: GetUserTaskRequest): Observable<UserTaskModel[]> {
    let params = mapGetUserTaskRequest(request)

    return this.http.get<UserTaskModel[]>(API_URL, { params })
  }

  public post(request: AddUserTaskRequest): Observable<any> {
    return this.http.post(API_URL, request)
  }

  public getById(id: number): Observable<UserTaskModel> {
    return this.http.get<UserTaskModel>(`${API_URL}/${id}`)
  }

  public deleteById(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/${id}`)
  }

  public update(request: UpdateUserTaskRequest): Observable<any> {
    return this.http.put(API_URL, request)
  }
}
