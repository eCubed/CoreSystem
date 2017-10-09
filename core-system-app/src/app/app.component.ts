import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { AuthService } from './services/auth.service';
import { LocalStorageService } from './services/local-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
}) 
export class AppComponent implements OnInit {
	isLoggedIn: boolean;
  loginInfoSubscription:Subscription;
  username:string;

  constructor(private authService:AuthService,
              private localStorageService:LocalStorageService,
              private router:Router){

    var loginInfo = localStorageService.resolveObject("loginInfo");

    this.isLoggedIn = (loginInfo != null);

    if (this.isLoggedIn)
      this.username = loginInfo.username;
    
    this.loginInfoSubscription = this.authService.loginInfo$
      .subscribe(loginInfo => {
        this.isLoggedIn = (loginInfo != null);

        if (this.isLoggedIn)
          this.username = loginInfo.username;        
    });
    
  }

  ngOnInit(){
  }

  ngOnDestroy() {
    if (this.loginInfoSubscription != null)
      this.loginInfoSubscription.unsubscribe();
  }

  onLogout(event) {
  	event.preventDefault();  	
  	this.authService.logout();
    this.router.navigate(['/login']);
  }
}
