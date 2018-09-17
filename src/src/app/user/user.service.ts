import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

@Injectable(

)
export class UserService {

  constructor(private http: Http) {
  }

  getUser(username) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    let data = { Username: username };
    return this.http.get('http://localhost:5000/api/User/GetUser', { params: data, headers })
      .map(response => response.json());
  }
}
