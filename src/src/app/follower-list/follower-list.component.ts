import { Component, OnInit } from '@angular/core';
import { FollowerListService } from './follower-list.service';
import { FollowedListDetails } from './FollowedListDetails';

@Component({
  selector: 'app-follower-list',
  templateUrl: './follower-list.component.html',
  styleUrls: ['./follower-list.component.scss']
})
export class FollowerListComponent implements OnInit {

  followedListDetail: FollowedListDetails;
  data: any;

  constructor(private followerListService: FollowerListService) { }

  ngOnInit() {
    this.followerListService.getFollowedList().subscribe((followedListDetail: FollowedListDetails) => {
      console.log(followedListDetail);
      this.data = followedListDetail;
    });

  }
}
