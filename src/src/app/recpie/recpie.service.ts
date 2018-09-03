import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

@Injectable(

)
export class RecpieService {

  constructor(private http: Http) {
  }

  getRecpieId(id) {
    let data = { Id: id };
    return this.http.get('http://localhost:5000/api/Recpie/GetRecpie', { params: data })
      .map(response => response.json());
  }

  postRating(data) {

    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return this.http.post('http://localhost:5000/api/Recpie/PostRating', data, { headers })
      .map(response => response.json());
  }
}
