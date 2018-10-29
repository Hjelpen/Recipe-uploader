import { Component, OnInit, NgZone } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserInformation } from '../models/user.information.interface';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ImageCroppedEvent } from 'ngx-image-cropper/src/image-cropper.component';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  userInformation: UserInformation;

  imageSrc: string = "";
  url = '';
  files: FileList;
  filestring: string = "";
  fileName: string = "";
  fileType: string = "";
  errors: string;
  data: any;
  bio: string = "";
  bioInfo: any;

  constructor(private dashboardService: DashboardService, private router: Router, private zone: NgZone) {
  }

  ngOnInit() {

    this.dashboardService.getProfile()
      .subscribe((userInformation: UserInformation) => {
        this.data = userInformation;
        this.bio = userInformation.bio;
        console.log(this.data);
      },
        error => {
        });
  }

  imageChangedEvent: any = '';
  croppedImage: any = '';

  fileChangeEvent(event: any, files: FileList): void {
    this.imageChangedEvent = event;

    this.files = event.target.files;
    this.fileName = this.files[0].name;
    this.fileType = this.files[0].type;

  }
  imageCropped(event: ImageCroppedEvent, files: FileList) {
    this.croppedImage = event.base64;
    this.filestring = this.croppedImage;
    console.log(this.fileName);
    console.log(this.fileType)
  }
  imageLoaded() {
  }
  loadImageFailed() {
  }

  saveProfilePicture() {

    let formData = {
      "File": this.filestring,
      "FileName": this.fileName,
      "FileType": this.fileType
    };
    this.dashboardService.saveProfilePicture(formData)
      .subscribe(result => {
        this.data = result;
        console.log(this.data)
      },
      error => this.errors = error);
  }

  saveBioProfile() {
    this.dashboardService.saveProfileBio(this.bio)
      .subscribe((userInformation: UserInformation) => {
        this.bio = userInformation.bio;
        console.log(this.data);
      },
      error => this.errors = error);
  }
}
