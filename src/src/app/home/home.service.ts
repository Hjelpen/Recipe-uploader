import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

@Injectable(

)
export class HomeService {

  constructor(private http: Http) {
  }

  getRecpies() {
    return this.http.get('http://localhost:5000/api/Recpie')
      .map(response => response.json());
  }

  getSortedNew() {
    return this.http.get('http://localhost:5000/api/Recpie/SortNew')
      .map(response => response.json());
  }

  getSearch(searchQuery) {
   let data = { SearchQuery: searchQuery };
    return this.http.get('http://localhost:5000/api/Recpie/Search', { params: data })
      .map(response => response.json());
  }
}

