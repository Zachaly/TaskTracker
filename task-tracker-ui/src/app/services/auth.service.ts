import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import LoginRequest from '../model/request/LoginRequest';
import LoginResponse from '../model/LoginResponse';

const API_URL = 'https://localhost:5001/api/auth'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userData?: LoginResponse = undefined

  constructor(private http: HttpClient) { }

  public login(request: LoginRequest): Observable<any> {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).pipe((res) => {
      res.subscribe({
        next: (res) => this.userData = res
      })
      return res
    })
  }

  public isAuthorized(): boolean {
    return this.userData !== undefined
  }
}
