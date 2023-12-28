import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import AddDocumentRequest from 'src/app/model/document/AddDocumentRequest';
import DocumentModel from 'src/app/model/document/DocumentModel';
import UserSpaceModel from 'src/app/model/user-space/UserSpaceModel';
import UpdateUserSpaceRequest from 'src/app/model/user-space/request/UpdateUserSpaceRequest';
import { AuthService } from 'src/app/services/auth.service';
import { DocumentService } from 'src/app/services/document.service';
import { UserSpaceService } from 'src/app/services/user-space.service';

@Component({
  selector: 'app-space-page',
  templateUrl: './space-page.component.html',
  styleUrls: ['./space-page.component.css']
})
export class SpacePageComponent implements OnInit {

  spaceId: number = 0

  space: UserSpaceModel = {
    id: 0,
    owner: {
      id: 0,
      firstName: '',
      lastName: '',
      email: ''
    },
    title: '',
    statusGroup: {
      id: 0,
      statuses: [],
      name: '',
      isDefault: false
    },
    lists: []
  }

  updateRequest: UpdateUserSpaceRequest = {
    id: 0,
    title: ''
  }

  documents: DocumentModel[] = []

  isUpdatingSpace = false

  newDocumentTitle = ''

  constructor(private spaceService: UserSpaceService, private route: ActivatedRoute, private router: Router,
    private documentService: DocumentService, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(param => {
      this.spaceId = param['id']
      this.loadSpace()
    })
  }

  private loadSpace() {
    this.spaceService.getById(this.spaceId).subscribe(res => {
      this.space = res
      this.updateRequest.id = res.id
      this.updateRequest.title = res.title
    })
    this.documentService.get({ spaceId: this.spaceId }).subscribe(res => this.documents = res)
  }

  update() {
    this.spaceService.update(this.updateRequest).subscribe(() => {
      this.loadSpace()
      this.isUpdatingSpace = false
    })
  }

  deleteSpace() {
    this.spaceService.deleteById(this.spaceId).subscribe(() => {
      this.router.navigate(['/'])
    })
  }

  addDocument() {
    const request: AddDocumentRequest = {
      title: this.newDocumentTitle,
      creatorId: this.authService.userData!.userData!.id,
      spaceId: this.spaceId
    }
    this.documentService.add(request).subscribe(res => {
      this.newDocumentTitle = ''

      this.documentService.getById(res.newEntityId!).subscribe(doc => this.documents.push(doc))
    })
  }

  deleteDocument(id: number) {
    this.documentService.deleteById(id).subscribe(() => this.documents = this.documents.filter(x => x.id != id))
  }
}
