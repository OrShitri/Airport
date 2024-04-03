import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LogsServiceService {

  url: string = "https://localhost:7253/api/Flights/";

  constructor(private httpServer:HttpClient) { }

    get(){
      return this.httpServer.get(this.url);
  }
}
