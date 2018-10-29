import { Component, OnInit, NgZone } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';
import { FormsModule  } from '@angular/forms';
import { DashboardService } from '../services/dashboard.service';
import { error } from 'protractor';
import { Router, ActivatedRoute } from '@angular/router';
import { ok } from 'assert';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ImageCroppedEvent } from 'ngx-image-cropper/src/image-cropper.component';

@Component({
  selector: 'app-newrecepie',
  templateUrl: './newrecepie.component.html',
  styleUrls: ['./newrecepie.component.scss']
})


export class NewrecepieComponent implements OnInit {

  title: string = "";
  ingredient: string = "";
  content: string = "";
  ingredients: string[] = [];
  imageSrc: string = "";
  url = '';
  files: FileList;
  filestring: string = "";
  fileName: string = "";
  fileType: string = "";
  errors: string;

  constructor(private dashboardService: DashboardService, private router: Router, private zone: NgZone) {
  }

  ngOnInit() {

  }

  addingredient() {
    this.ingredients.push(this.ingredient);
    this.ingredient = "";
  }

  removeingredient(index) {
    this.ingredients.splice(index, 1);
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

  saveRecpie() {

    let formData = {
      "title": this.title,
      "content": this.content,
      "ingredients": this.ingredients,
      "file": this.filestring,
      "fileName": this.fileName,
      "fileType": this.fileType
    };

    this.dashboardService.addNewRecepie(formData)
      .subscribe(result => {
        return this.zone.run(() => this.router.navigate(['/dashboard/home']));
      },
       error => this.errors = error);
    }
}

