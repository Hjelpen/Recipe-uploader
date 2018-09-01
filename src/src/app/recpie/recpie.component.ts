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

  id: string = "";
  recpieDetail: RecpieDetail;
  data: any;
  onClickResult: ClickEvent;
  recpieId = this.route.snapshot.paramMap.get('id');

  constructor(private route: ActivatedRoute, private recpieService: RecpieService) { }

  ngOnInit() {
    this.getRecpie();
  }

  getRecpie() {
    this.recpieService.getRecpieId(this.recpieId).subscribe((recpieDetail: RecpieDetail) => {
      this.data = recpieDetail;

      console.log(this.data);
      });
  }

  onClick = ($event: ClickEvent) => {
    console.log('onClick $event: ', $event);

    this.recpieService.postRating($event, this.recpieId).subscribe();
  };

}
