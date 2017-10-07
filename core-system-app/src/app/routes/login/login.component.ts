import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { AuthService } from '../../services/auth.service';
import { LocalStorageService } from '../../services/local-storage.service';

import { LoginStatus } from '../../services/models/loginstatus';

import { fadeInAnimation } from '../../animations/animations';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [fadeInAnimation],
  host: { '[@fadeInAnimation]': ''}
})
export class LoginComponent implements OnInit {

  credentials = {
    username: "",
    password: ""
  };

  loginStatusListener:Subscription;
  loginStatus:LoginStatus;

  constructor(private authService:AuthService,
              private localStorageService:LocalStorageService,
              private router:Router) {

    this.loginStatus = this.authService.getLoginStatus();

    this.loginStatusListener = this.authService.getLoginStatusDispatcher().subscribe(loginStatus => {
      this.loginStatus = loginStatus;
      this.router.navigate(['/dashboard']);
    });
  }

  ngOnInit() {
  }

  login(){
  	this.authService.login(this.credentials.username, this.credentials.password);
  	// For now, always forward to the protected page.
  }

}
