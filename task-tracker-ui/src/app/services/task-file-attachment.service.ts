import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetTaskFileAttachmentRequest, { mapGetTaskFileAttachmentRequest } from '../model/task-file-attachment/GetTaskFileAttachmentRequest';
import TaskFileAttachmentModel from '../model/task-file-attachment/TaskFileAttachmentModel';

const API_URL = 'https://localhost:5001/api/task-file-attachment'

@Injectable({
  providedIn: 'root'
})
export class TaskFileAttachmentService {

  constructor(private http: HttpClient) { }

  get(request: GetTaskFileAttachmentRequest) {
    const params = mapGetTaskFileAttachmentRequest(request)

    return this.http.get<TaskFileAttachmentModel[]>(API_URL, { params })
  }

  getById(id: number) {
    return this.http.get<TaskFileAttachmentModel>(`${API_URL}/${id}`)
  }

  post(taskId: number, files: File[]) {
    const body = new FormData()

    body.append("TaskId", taskId.toString())
    
    files.forEach(f => body.append('Files', f))

    return this.http.post(API_URL, body)
  }

  deleteById(id: number) {
    return this.http.delete(`${API_URL}/${id}`)
  }
}
