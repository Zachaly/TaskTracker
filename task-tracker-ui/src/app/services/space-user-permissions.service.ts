import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetSpaceUserPermissionsRequest, { mapGetSpaceUserPermissionsRequest } from '../model/space-user-permissions/GetSpaceUserPermissionsRequest';
import SpaceUserPermissionsModel from '../model/space-user-permissions/SpaceUserPermissionsModel';
import AddSpaceUserPermissionsRequest from '../model/space-user-permissions/AddSpaceUserPermissionsRequest';
import UpdateSpaceUserPermissionsRequest from '../model/space-user-permissions/UpdateSpaceUserPermissionsRequest';

const API_URL = 'https://localhost:5001/api/space-user-permissions'

@Injectable({
  providedIn: 'root'
})
export class SpaceUserPermissionsService {

  constructor(private http: HttpClient) { }

  get(request: GetSpaceUserPermissionsRequest) {
    const params = mapGetSpaceUserPermissionsRequest(request)

    return this.http.get<SpaceUserPermissionsModel[]>(API_URL, { params })
  }

  add(request: AddSpaceUserPermissionsRequest) {
    return this.http.post(API_URL, request, { headers: { 
      'SpaceId': request.spaceId.toString()
    }})
  }

  update(request: UpdateSpaceUserPermissionsRequest) {
    return this.http.put(API_URL, request, { headers: {
      'SpaceId': request.spaceId.toString()
    }})
  }
}
