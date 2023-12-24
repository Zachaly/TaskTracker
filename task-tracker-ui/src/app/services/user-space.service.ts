import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetUserSpaceRequest, { mapGetUserSpaceRequest } from '../model/user-space/request/GetUserSpaceRequest';
import UserSpaceModel from '../model/user-space/UserSpaceModel';
import AddUserSpaceRequest from '../model/user-space/request/AddUserSpaceRequest';
import { CreatedResponseModel } from '../model/ResponseModel';
import UpdateUserSpaceRequest from '../model/user-space/request/UpdateUserSpaceRequest';

const API_URL = 'https://localhost:5001/api/user-space'

@Injectable({
  providedIn: 'root'
})
export class UserSpaceService {

  constructor(private http: HttpClient) { }

  get(request: GetUserSpaceRequest) {
    const params = mapGetUserSpaceRequest(request)

    return this.http.get<UserSpaceModel[]>(API_URL, { params })
  }

  getById(id: number) {
    return this.http.get<UserSpaceModel>(`${API_URL}/${id}`)
  }

  add(request: AddUserSpaceRequest) {
    return this.http.post<CreatedResponseModel>(API_URL, request)
  }

  update(request: UpdateUserSpaceRequest) {
    return this.http.put(API_URL, request)
  }

  deleteById(id: number) {
    return this.http.delete(`${API_URL}/${id}`)
  }
}
