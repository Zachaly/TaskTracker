import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import RegisterRequest from '../model/request/RegisterRequest';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/user'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public register(request: RegisterRequest) : Observable<any> {
    return this.http.post(API_URL, request)
  }
}
