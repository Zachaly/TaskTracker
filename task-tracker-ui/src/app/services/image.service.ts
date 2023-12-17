import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

const API_URL = 'https://localhost:5001/api/image'

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) { }

  public updateProfilePicture(id: number, file?: File) {
    const formData = new FormData()

    formData.append('UserId', id.toString())

    if(file){
      formData.append('File', file)
    }

    return this.http.post('https://localhost:5001/api/image/user', formData)
  }

  public profilePicturePath(userId: number) : string {
    return `${API_URL}/user/${userId}`
  }
}
