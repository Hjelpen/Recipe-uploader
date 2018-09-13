import { Component, OnInit, NgZone } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  imageSrc: string = "";
  url = '';
  files: FileList;
  filestring: string = "";
  fileName: string = "";
  fileType: string = "";
  errors: string;
  data: any;

  constructor(private dashboardService: DashboardService, private router: Router, private zone: NgZone) {
  }

  ngOnInit() {

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

}
