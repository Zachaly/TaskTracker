import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import LoginRequest from '../model/request/LoginRequest';
import LoginResponse from '../model/LoginResponse';

const API_URL = 'https://localhost:5001/api/auth'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userData?: LoginResponse = undefined
  public userDataSubject = new Subject<LoginResponse>()
  private rememberMe = false
  public isRefreshingToken = false

  constructor(private http: HttpClient) { }

  public login(request: LoginRequest, rememberMe = false): Observable<any> {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).pipe(tap({
      next: (res) => {
        this.rememberMe = rememberMe
        this.userData = res
        this.userDataSubject.next(res)
        this.saveUserData()
      }
    }))
  }

  public isAuthorized(): boolean {
    return this.userData !== undefined
  }

  public refreshToken() {
    if(this.isRefreshingToken) {
      return
    }

    this.isRefreshingToken = true
    return this.http.post<LoginResponse>(`${API_URL}/refresh-token`, {
      accessToken: this.userData?.accessToken,
      refreshToken: this.userData?.refreshToken
    }).subscribe({
      next: (res) => {
        this.userData = res
        this.userDataSubject.next(res)
        if(this.rememberMe) {
          this.saveUserData()
        }
        this.isRefreshingToken = false
      }, 
      error: () => this.isRefreshingToken = false
    })
  }

  public revokeToken() {
    return this.http.put(`${API_URL}/revoke-token`, {
      refreshToken: this.userData?.refreshToken
    }).subscribe({
      next: () => this.clearUserData()
    })
  }

  public saveUserData() {
    if (!this.rememberMe) {
      return
    }

    localStorage.setItem('access_token', this.userData!.accessToken!)
    localStorage.setItem('refresh_token', this.userData!.refreshToken!)
  }

  private clearUserData() {
    this.userData = undefined
    this.rememberMe = false

    localStorage.setItem('access_token', '')
    localStorage.setItem('refresh_token', '')
  }

  public loadUserData() {
    const refreshToken = localStorage.getItem('refresh_token')
    const accessToken = localStorage.getItem('access_token')

    if (!refreshToken || !accessToken || this.isRefreshingToken) {
      return
    }
    this.rememberMe = true
    this.isRefreshingToken = true

    this.http.post<LoginResponse>(`${API_URL}/refresh-token`, {
      accessToken,
      refreshToken
    }).subscribe({
      next: (res) => {
        this.userData = res
        this.saveUserData()
        this.userDataSubject.next(res)
        this.isRefreshingToken = false
      },
      error: () =>  {
        this.isRefreshingToken = false
        this.clearUserData()
      }
    })
  }
}
