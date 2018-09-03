import { Component } from '@angular/core';
import { HomeService } from '../home/home.service';
import { HomeDetails } from '../dashboard/models/home.details.interface';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  homeDetails: HomeDetails;
  data: any;
  searchQuery: string = "";

  constructor(private homeService: HomeService) { }

  ngOnInit() {
    this.homeService.getRecpies().subscribe((homeDetails: HomeDetails) => {
      this.data = homeDetails;
      console.log(homeDetails)
    })
  };

  SortNew() {
    this.homeService.getSortedNew().subscribe((homeDetails: HomeDetails) => {
      this.data = homeDetails;
    })
  };

  SortRated() {
    this.homeService.getSortedRating().subscribe((homeDetails: HomeDetails) => {
      this.data = homeDetails;
      console.log(this.data);
    })
  };

  Search() {
    this.homeService.getSearch(this.searchQuery).subscribe((homeDetails: HomeDetails) => {
      this.data = homeDetails;
      this.searchQuery = "";
      console.log(this.searchQuery);
    })
  };
}

