import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AuthService } from '../services/auth.service'; 
import { ArraysService } from '../services/arrays.service';

@Injectable()
export class AuthGuard implements CanActivate {

	constructor(private authService:AuthService,
							private router:Router,
              private arraysService:ArraysService){

	}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    if(this.authService.isLoggedIn()) {
      let roles = next.data["roles"] as Array<string>;

      if (roles == null)
        return true;

      var canAccessPage = this.arraysService.anyOfFirstIsInSecond(this.authService.getLoginInfo().roles, roles);

    	if (canAccessPage)
        return true;

      this.router.navigate(['/unauthorized']);
      return false;
    }

    // not logged in
    this.router.navigate(['/login']);
    return false;
    
  }
}
