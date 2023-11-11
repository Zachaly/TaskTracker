import { Injectable } from '@angular/core';

const REFRESH_TOKEN = 'refresh_token'
const ACCESS_TOKEN = 'access_token'

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  getAccessToken() {
    return sessionStorage.getItem('access_token')
  }

  getRefreshToken() {
    return sessionStorage.getItem(REFRESH_TOKEN)
  }

  setSessionTokens(accessToken: string, refreshToken: string) {
    sessionStorage.setItem(ACCESS_TOKEN, accessToken)
    sessionStorage.setItem(REFRESH_TOKEN, refreshToken)
  }

  saveTokens() {
    localStorage.setItem(REFRESH_TOKEN, this.getRefreshToken() ?? '')
    localStorage.setItem(ACCESS_TOKEN, this.getAccessToken() ?? '')
  }

  clearTokens() {
    this.setSessionTokens('', '')
    this.saveTokens()
  }

  getStorageTokens() {
    const accessToken = localStorage.getItem(ACCESS_TOKEN)
    const refreshToken = localStorage.getItem(REFRESH_TOKEN)

    return { accessToken, refreshToken }
  }
}
