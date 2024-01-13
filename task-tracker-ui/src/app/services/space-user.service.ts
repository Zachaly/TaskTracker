import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetSpaceUserRequest, { mapGetSpaceUserRequest } from '../model/space-user/GetSpaceUserRequest';
import SpaceUserModel from '../model/space-user/SpaceUserModel';
import AddSpaceUserRequest from '../model/space-user/AddSpaceUserRequest';

const API_URL = 'https://localhost:5001/api/space-user'

@Injectable({
  providedIn: 'root'
})
export class SpaceUserService {

  constructor(private http: HttpClient) { }

  get(request: GetSpaceUserRequest){
    const params = mapGetSpaceUserRequest(request)

    return this.http.get<SpaceUserModel[]>(API_URL, { params })
  }

  add(request: AddSpaceUserRequest) {
    return this.http.post(API_URL, request)
  }

  delete(spaceId: number, userId: number) {
    return this.http.delete(`${API_URL}/${spaceId}/${userId}`)
  }
}
