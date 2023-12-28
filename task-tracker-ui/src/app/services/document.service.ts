import { Injectable } from '@angular/core';
import GetDocumentRequest, { mapGetDocumentRequest } from '../model/document/GetDocumentRequest';
import { HttpClient } from '@angular/common/http';
import DocumentModel from '../model/document/DocumentModel';
import AddDocumentRequest from '../model/document/AddDocumentRequest';
import UpdateDocumentRequest from '../model/document/UpdateDocumentRequest';
import { CreatedResponseModel } from '../model/ResponseModel';

const API_URL = 'https://localhost:5001/api/document'

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(private http: HttpClient) { }

  public get(request: GetDocumentRequest) {
    const params = mapGetDocumentRequest(request)

    return this.http.get<DocumentModel[]>(API_URL, { params })
  }

  public getById(id: number) {
    return this.http.get<DocumentModel>(`${API_URL}/${id}`)
  }

  public add(request: AddDocumentRequest) {
    return this.http.post<CreatedResponseModel>(API_URL, request)
  }

  public update(request: UpdateDocumentRequest) {
    return this.http.put(API_URL, request)
  }

  public deleteById(id: number) {
    return this.http.delete(`${API_URL}/${id}`)
  }
}
