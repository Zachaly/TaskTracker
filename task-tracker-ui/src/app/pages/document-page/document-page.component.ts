import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import AddDocumentPageRequest from 'src/app/model/document-page/AddDocumentPageRequest';
import DocumentPageModel from 'src/app/model/document-page/DocumentPageModel';
import UpdateDocumentPageRequest from 'src/app/model/document-page/UpdateDocumentPageRequest';
import DocumentModel from 'src/app/model/document/DocumentModel';
import UpdateDocumentRequest from 'src/app/model/document/UpdateDocumentRequest';
import { DocumentPageService } from 'src/app/services/document-page.service';
import { DocumentService } from 'src/app/services/document.service';

@Component({
  selector: 'app-document-page',
  templateUrl: './document-page.component.html',
  styleUrls: ['./document-page.component.css']
})
export class DocumentPageComponent implements OnInit {

  document: DocumentModel = {
    title: '',
    creator: {
      firstName: '',
      lastName: '',
      email: '',
      id: 0
    },
    pages: [],
    id: 0,
    creationTimestamp: 0
  }

  currentPage: DocumentPageModel = {
    id: 0,
    lastModifiedTimestamp: 0,
    content: '',
  }

  getDate = (timestamp: number) => new Date(timestamp)

  updatePageRequest: UpdateDocumentPageRequest = {
    id: 0,
  }

  isUpdatingPageTitle = false

  isUpdatingDocumentTitle = false

  updateDocumentRequest: UpdateDocumentRequest = {
    id: 0,
  }

  constructor(private route: ActivatedRoute, private documentService: DocumentService, private documentPageService: DocumentPageService) {

  }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      const documentId = params['id']

      this.documentService.getById(documentId).subscribe(res => {
        this.document = res
        this.selectPage(res.pages[0])

        this.updateDocumentRequest = {
          id: documentId,
          title: this.document.title
        }
      })
    })
  }

  selectPage(page: DocumentPageModel) {
    this.currentPage = page

    this.updatePageRequest = {
      id: page.id,
      content: page.content,
      title: page.title
    }
  }

  updatePage() {
    this.documentPageService.update(this.updatePageRequest).subscribe(() => {
      this.documentPageService.getById(this.currentPage.id).subscribe(res => {
        this.document.pages[this.document.pages.findIndex(x => x == this.currentPage)] = res
        this.selectPage(res)
      })
    })
  }

  updatePageTitle() {
    this.isUpdatingPageTitle = false
    this.updatePage()
  }

  changePage(page: DocumentPageModel) {
    if (this.currentPage.content != this.updatePageRequest.content) {
      const conf = confirm('You have unsaved changes. Do you want to discard them?')
      if (!conf) {
        return
      }
    }

    this.selectPage(page)
  }

  addPage() {
    const request: AddDocumentPageRequest = {
      documentId: this.document.id,
      content: ''
    }

    this.documentPageService.add(request).subscribe(res => {
      this.documentPageService.getById(res.newEntityId!).subscribe(newPage => {
        this.document.pages.push(newPage)
        this.changePage(newPage)
      })
    })
  }

  updateDocumentTitle() {
    this.documentService.update(this.updateDocumentRequest).subscribe(() => {
      this.document.title = this.updateDocumentRequest.title!
      this.isUpdatingDocumentTitle = false
    })
  }

  deletePage(page: DocumentPageModel) {
    if(this.document.pages.length <= 1) {
      alert('Document must have at least one page!')
      return;
    }

    this.documentPageService.deleteById(page.id).subscribe(() => {
      this.document.pages = this.document.pages.filter(x => x.id !== page.id)
      if(page == this.currentPage) {
        this.selectPage(this.document.pages[0])
      }
    })
  }
}
