import { Component, OnInit } from '@angular/core';

import { HomeDetails } from '../models/home.details.interface';
import { DashboardService } from '../services/dashboard.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  homeDetails: HomeDetails;
  items: any;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {

    this.dashboardService.getHomeDetails()
      .subscribe((homeDetails: HomeDetails) => {
        console.log(homeDetails)
        homeDetails.imageUrl
        this.items = homeDetails;
        console.log(this.items);
      },
      error => {
        console.log(error);
        });

  }

  Delete(id, title) {
    if (confirm("Are you sure to delete " + title)) {
      this.dashboardService.deleteRecpie(id).subscribe((homeDetails: HomeDetails) => {
        homeDetails.imageUrl
        this.items = homeDetails;
      });
    }
  }
}
