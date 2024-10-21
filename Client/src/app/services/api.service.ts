import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../models/model';

@Injectable({
  providedIn: 'root'
})
export class ApiServiceService {
  status: Subject<string>  = new Subject();
  baseURL = "http://localhost:5294/api/";
  constructor(private httpClient: HttpClient, private jwt: JwtHelperService) { }

  login(user: any ){
    return this.httpClient.post(this.baseURL+"auth/login", user,{responseType:"text"})
  }

  register(user: any){
    return this.httpClient.post(this.baseURL+"auth/register", user, {responseType:"text"})
  }

  isLoggedIn() : boolean{
    if(localStorage.getItem("access_token") && !this.jwt.isTokenExpired()){
      return true
    }
    return false
  }

  getUserInfo() : User | null{
    if(this.isLoggedIn()){
      let decodedToken = this.jwt.decodeToken()
      let user : User = {
        id: decodedToken.id,
        name:decodedToken.name,
        username: decodedToken.username,
        password: ""
      }

      return user;
    }
    return null;
  }
}
