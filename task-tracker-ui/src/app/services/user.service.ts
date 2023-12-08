import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import RegisterRequest from '../model/user/RegisterRequest';
import { Observable } from 'rxjs';
import UpdateUserRequest from '../model/user/UpdateUserRequest';
import UserModel from '../model/user/UserModel';

const API_URL = 'https://localhost:5001/api/user'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public register(request: RegisterRequest): Observable<any> {
    return this.http.post(API_URL, request)
  }

  public update(request: UpdateUserRequest): Observable<any> {
    return this.http.put(API_URL, request)
  }

  public getById(id: number) : Observable<UserModel> {
    return this.http.get<UserModel>(`${API_URL}/${id}`)
  }
}
