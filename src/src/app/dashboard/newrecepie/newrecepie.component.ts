import { Component, OnInit } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';
import { FormsModule  } from '@angular/forms';
import { DashboardService } from '../services/dashboard.service';
import { error } from 'protractor';
import { Router, ActivatedRoute } from '@angular/router';

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

  constructor(private dashboardService: DashboardService, private router: Router) {
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

  onSelectFile(event, files: FileList) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]);

      reader.onload = (imgsrc: any) => {
        this.url = imgsrc.target.result;

      }

      this.files = event.target.files;
      var reader = new FileReader();
      reader.onload = this._handleReaderLoaded.bind(this);
      reader.readAsBinaryString(this.files[0]);
      this.fileName = this.files[0].name;
      this.fileType = this.files[0].type;
    }
  }

  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.filestring = btoa(binaryString);
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
      .subscribe(
        result => {
          if (result) {
            this.router.navigate(['/dashboard/home']);
          }
        });
  }
}
