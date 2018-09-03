import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecpieService } from '../recpie/recpie.service';
import { RecpieDetail } from '../recpie/recpie.interface';
import { ClickEvent, HoverRatingChangeEvent, RatingChangeEvent } from 'angular-star-rating';

@Component({
  selector: 'app-recpie',
  templateUrl: './recpie.component.html',
  styleUrls: ['./recpie.component.scss']
})
export class RecpieComponent implements OnInit {

  recpieDetail: RecpieDetail;
  data: any;
  onClickResult: ClickEvent;
  recpieId = this.route.snapshot.paramMap.get('id');
  clickRating: number = 0;
  id: number = 0;


  constructor(private route: ActivatedRoute, private recpieService: RecpieService) { }

  ngOnInit() {
    this.getRecpie();
  }

  getRecpie() {
    this.recpieService.getRecpieId(this.recpieId).subscribe((recpieDetail: RecpieDetail) => {
      recpieDetail.averageRating = recpieDetail.rating / recpieDetail.totalVotes;
      console.log(recpieDetail.totalVotes)
      this.data = recpieDetail;
      });
  }

  onClick = ($event: ClickEvent) => {
    this.clickRating = $event.rating;
    this.id = parseInt(this.recpieId);

    let data = {
      Rating: this.clickRating,
      Id: this.id
    };

    this.recpieService.postRating(data).subscribe();
  };

}
