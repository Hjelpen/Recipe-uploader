import { Component, OnInit } from '@angular/core';
import { UserService } from './user.service';
import { UserDetails } from '../shared/models/userDetail.interface';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  userDetails: UserDetails;
  username: string = this.route.snapshot.paramMap.get('username');
  data: any;

  constructor(private userService: UserService, private route: ActivatedRoute,) { }

  ngOnInit() {
    console.log(this.username)
    this.userService.getUser(this.username).subscribe((userDetail: UserDetails) => {
      this.data = userDetail;
      console.log(this.data)
    });
  }

}
