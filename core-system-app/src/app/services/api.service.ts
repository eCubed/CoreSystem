import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class ApiService {

	public readonly API_ROOT: string = 'http://localhost:49550/api/';
	public readonly TOKEN_ENDPOINT: string = 'http://localhost:49197/token';

  constructor(private localStorageService: LocalStorageService,
              private http: HttpClient) { 
  }
	
	private buildAuthorizationHeader() {

		const loginInfo = this.localStorageService.resolveObject("loginInfo");
		var accessToken = (loginInfo != null) ? loginInfo.accessToken : "";

		return {
			headers: new HttpHeaders()
				.set('Authorization', `Bearer ${accessToken}`)
		};
	}  

}
