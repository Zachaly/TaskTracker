import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import GetUserTaskRequest, { mapGetUserTaskRequest } from '../model/request/get/GetUserTaskRequest';
import { Observable } from 'rxjs';
import UserTaskModel from '../model/UserTaskModel';
import AddUserTaskRequest from '../model/request/get/AddUserTaskRequest';

const API_URL = 'https://localhost:5001/api/user-task'

@Injectable({
  providedIn: 'root'
})
export class UserTaskService {

  private httpHeaders = () =>
    new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.userData?.accessToken}`
    })

  constructor(private http: HttpClient, private authService: AuthService) { }

  public get(request: GetUserTaskRequest) : Observable<UserTaskModel[]> {
    let params = mapGetUserTaskRequest(request)

    return this.http.get<UserTaskModel[]>(API_URL, { params, headers: this.httpHeaders() })
  }

  public post(request: AddUserTaskRequest) : Observable<any> {
    return this.http.post(API_URL, request, {
      headers: this.httpHeaders()
    })
  }

  public getById(id: number) : Observable<UserTaskModel> {
    return this.http.get<UserTaskModel>(`${API_URL}/${id}`, {
      headers: this.httpHeaders()
    })
  }

  public deleteById(id: number) : Observable<any> {
    return this.http.delete(`${API_URL}/${id}`, {
      headers: this.httpHeaders()
    })
  }
}
