import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import LoginRequest from '../model/user/LoginRequest';
import LoginResponse from '../model/user/LoginResponse';
import { TokenService } from './token.service';
import ChangeUserPasswordRequest from '../model/user/ChangeUserPasswordRequest';

const API_URL = 'https://localhost:5001/api/auth'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userData?: LoginResponse = undefined
  public userDataSubject = new Subject<LoginResponse>()
  private rememberMe = false
  public isRefreshingToken = false

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  public login(request: LoginRequest, rememberMe = false): Observable<any> {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).pipe(tap({
      next: (res) => {
        this.rememberMe = rememberMe
        this.userData = res
        this.userDataSubject.next(res)
        this.setTokens()
      }
    }))
  }

  public changeUserPassword(request: ChangeUserPasswordRequest) {
    return this.http.patch(`${API_URL}/change-password`, request).pipe(tap({
      next: () => this.clearUserData()
    }))
  }

  public isAuthorized(): boolean {
    return this.userData !== undefined
  }

  public refreshToken() {
    if (this.isRefreshingToken) {
      return
    }

    this.isRefreshingToken = true
    return this.http.post<LoginResponse>(`${API_URL}/refresh-token`, {
      accessToken: this.tokenService.getAccessToken(),
      refreshToken: this.tokenService.getRefreshToken()
    }).pipe(tap({
      next: (res) => {
        this.userData = res
        this.userDataSubject.next(res)
        this.setTokens()
        console.log(res)
        this.isRefreshingToken = false
      },
      error: () => this.isRefreshingToken = false
    }))
  }

  public revokeToken() {
    return this.http.put(`${API_URL}/revoke-token`, {
      refreshToken: this.userData?.refreshToken
    }).subscribe({
      next: () => this.clearUserData()
    })
  }

  private clearUserData() {
    this.userData = undefined
    this.rememberMe = false

    this.tokenService.clearTokens()
  }

  private setTokens() {
    const { accessToken, refreshToken } = this.userData!

    this.tokenService.setSessionTokens(accessToken, refreshToken);
    if (this.rememberMe) {
      this.tokenService.saveTokens()
    }
  }

  public loadUserData() {
    const { accessToken, refreshToken } = this.tokenService.getStorageTokens()

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
        this.userDataSubject.next(res)
        this.setTokens()
        this.isRefreshingToken = false
      },
      error: () => {
        this.isRefreshingToken = false
        this.clearUserData()
      }
    })
  }
}
