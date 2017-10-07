import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';

import { LocalStorageService } from './local-storage.service';
import { ApiService } from './api.service';

import { LoginInfo } from './models/logininfo';
import { LoginStatus } from './models/loginstatus';

@Injectable()
export class AuthService {

  private loginInfo$: Subject<LoginInfo> = new Subject<LoginInfo>();
  private loginStatus$: Subject<LoginStatus> = new Subject<LoginStatus>();

  private loginInfo: LoginInfo;

  private loginStatuses: Array<LoginStatus>;

  private loginStatus: LoginStatus;

  constructor(private localStorageService: LocalStorageService,
              private apiService:ApiService,
              private http:Http) { 

    this.loginInfo = localStorageService.resolveObject("loginInfo");
    this.loginInfo$.next(this.loginInfo);

    this.loginStatuses = [
      new LoginStatus(LoginStatus.LOGGED_OUT, false, false),
      new LoginStatus(LoginStatus.LOGGING_IN, false, false),
      new LoginStatus(LoginStatus.LOGGED_IN, true, false),
      new LoginStatus(LoginStatus.LOGGING_OUT, true, false),
      new LoginStatus(LoginStatus.INVALID_LOGIN, false, true),
      new LoginStatus(LoginStatus.SERVER_ERROR, false, true),
      new LoginStatus(LoginStatus.SERVER_UNAVAILABLE, false, true)
    ];

    this.loginStatus = localStorageService.resolveObject("loginStatus");

    if (this.loginStatus == null)
      this.loginStatus = this.loginStatuses[0];

    this.loginStatus$.next(this.loginStatus);
  }

  findLoginStatus(name:string){
    var loginStatus:LoginStatus = null;

    for(var i = 0; i < this.loginStatuses.length; i++){
      if (this.loginStatuses[i].name == name){
        loginStatus = this.loginStatuses[i];
        break;
      }
    }

    return loginStatus;
  }

  getLoginInfo = () => this.loginInfo;

  getLoginStatusDispatcher = () => this.loginStatus$.asObservable();

  login = (username: string, password: string) => {

    var loggingInStatus = this.findLoginStatus(LoginStatus.LOGGING_IN);
    this.loginStatus$.next(loggingInStatus);
    this.localStorageService.setObject("loginStatus", loggingInStatus);
    
    var requestOptions = new RequestOptions({
       headers: new Headers({ 'Content-Type':'application/x-www-form-urlencoded'})
    });

    this.http.post(this.apiService.TOKEN_ENDPOINT, 
      `username=${username}&password=${password}&grant_type=password`, 
       requestOptions)
      .map(res => res.json())
      .subscribe((data) => 
      {
        this.loginInfo = new LoginInfo();
        this.loginInfo.accessToken = data.accessToken
        this.loginInfo.username = username;
        this.loginInfo.roles = data.roles;

        this.loginInfo$.next(this.loginInfo);
        this.localStorageService.setObject("loginInfo", this.loginInfo);

        this.loginStatus = this.findLoginStatus(LoginStatus.LOGGED_IN);
        this.loginStatus$.next(this.loginStatus);

        this.localStorageService.setObject("loginStatus", this.loginStatus);
      },
      (error)=>{
        console.log(JSON.stringify(error));

        if (error.status == 0)
          this.loginStatus = this.findLoginStatus(LoginStatus.SERVER_UNAVAILABLE);
        else if (error.status == 401)
          this.loginStatus = this.findLoginStatus(LoginStatus.INVALID_LOGIN);
        else if (error.status == 500)
          this.loginStatus = this.findLoginStatus(LoginStatus.SERVER_ERROR);

        this.loginStatus = this.loginStatus;
        this.loginStatus$.next(this.loginStatus);
        this.localStorageService.setObject("loginStatus", this.loginStatus);
     });
  }

  logout = () => {
    this.loginStatus$.next(this.loginStatuses[3]);
    this.localStorageService.setObject("loginStatus",  this.findLoginStatus(LoginStatus.LOGGING_OUT));

    this.loginInfo = null;
    this.localStorageService.remove("loginInfo");
  	this.loginInfo$.next(null);

    this.loginStatus = this.loginStatuses[0];
    this.loginStatus$.next(this.loginStatus);
    this.localStorageService.setObject("loginStatus", this.findLoginStatus(LoginStatus.LOGGED_OUT));
  }

  isLoggedIn = () => (this.loginStatus.isLoggedIn);

  getLoginStatus = () => this.loginStatus;
}
