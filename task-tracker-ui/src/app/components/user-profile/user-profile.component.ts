import { Component, Input, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  @Input() size = ''
  @Input() id = 0
  @Input() isRounded = false

  @ViewChild('template', { static: true }) template?: TemplateRef<HTMLElement>;

  constructor(private viewContainerRef: ViewContainerRef, private imageService: ImageService) { }

  ngOnInit() {
    this.viewContainerRef.createEmbeddedView(this.template!)
  }

  getSize() {
    return `is-${this.size ?? '64x64'}`
  }

  getUrl() {
    return this.imageService.profilePicturePath(this.id)
  }
}
