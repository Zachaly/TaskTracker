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

  public login(request: LoginRequest, remeberMe = false): Observable<any> {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).pipe((res) => {
      res.subscribe({
        next: (res) => {
          if(remeberMe) {
            this.saveUserData(res)
          } else {
            this.userData = res
          }
        }
      })
      return res
    })
  }

  public isAuthorized(): boolean {
    return this.userData !== undefined
  }

  public refreshToken() {
    return this.http.post<LoginResponse>(`${API_URL}/refresh-token`, {
      accessToken: this.userData?.accessToken,
      refreshToken: this.userData?.refreshToken
    }).subscribe({
      next: (res) => this.saveUserData(res)
    })
  }

  public revokeToken() {
    return this.http.put(`${API_URL}/revoke-token`, {
      refreshToken: this.userData?.refreshToken
    }).subscribe({
      next: () => this.clearUserData()
    })
  }

  private saveUserData(data: LoginResponse) {
    this.userData = data

    localStorage.setItem('access_token', data.accessToken!)
    localStorage.setItem('refresh_token', data.refreshToken!)
  }

  private clearUserData() {
    this.userData = undefined
    
    localStorage.setItem('access_token', '')
    localStorage.setItem('refresh_token', '')
  }

  public loadUserData() {
    const refreshToken = localStorage.getItem('refresh_token')
    const accessToken = localStorage.getItem('access_token')

    if(!refreshToken || !accessToken) {
      return
    }

    return this.http.post<LoginResponse>(`${API_URL}/refresh-token`, {
      accessToken,
      refreshToken
    }).pipe(res => {
      res.subscribe({
        next: (res) => this.saveUserData(res)
      })

      return res
    })
  }
}
