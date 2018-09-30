import { Component, OnInit } from '@angular/core';
import { UserService1 } from './user.service';
import { UserDetails } from '../shared/models/userDetail.interface';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../shared/services/user.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  userDetails: UserDetails;
  username: string = this.route.snapshot.paramMap.get('username');
  data: any;
  status: boolean;
  subscription: Subscription;
  errors: any;
  follow: boolean;

  constructor(private userService1: UserService1, private route: ActivatedRoute, private userService: UserService,) { }

  ngOnInit() {
    this.userService1.getUser(this.username).subscribe((userDetail: UserDetails) => {
      this.data = userDetail;
      console.log(this.data)
    });

    this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
  }

  Follow(userName) {
    this.userService1.FollowUser(userName).subscribe(result => {
      console.log(result)
      this.follow = true;
    },
      error => this.errors = error);
  }

}
