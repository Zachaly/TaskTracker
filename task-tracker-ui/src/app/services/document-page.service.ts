import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import GetDocumentPageRequest, { mapGetDocumentPageRequest } from '../model/document-page/GetDocumentPageRequest';
import DocumentPageModel from '../model/document-page/DocumentPageModel';
import AddDocumentPageRequest from '../model/document-page/AddDocumentPageRequest';
import UpdateDocumentPageRequest from '../model/document-page/UpdateDocumentPageRequest';
import { CreatedResponseModel } from '../model/ResponseModel';

const API_URL = 'https://localhost:5001/api/document-page'

@Injectable({
  providedIn: 'root'
})
export class DocumentPageService {

  constructor(private http: HttpClient) { }

  public get(request: GetDocumentPageRequest) {
    const params = mapGetDocumentPageRequest(request)

    return this.http.get<DocumentPageModel[]>(API_URL, { params })
  }

  public getById(id: number) {
    return this.http.get<DocumentPageModel>(`${API_URL}/${id}`)
  }

  public add(request: AddDocumentPageRequest) {
    return this.http.post<CreatedResponseModel>(API_URL, request)
  }

  public update(request: UpdateDocumentPageRequest) {
    return this.http.put(API_URL, request)
  }

  public deleteById(id: number){
    return this.http.delete(`${API_URL}/${id}`)
  }
}
