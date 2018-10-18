import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

@Injectable(
)
export class FollowerListService {

  constructor(private http: Http) {
  }

  getFollowedList() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return this.http.get('http://localhost:5000/api/User/GetFollowedList', { headers })
      .map(response => response.json())
  }
}
