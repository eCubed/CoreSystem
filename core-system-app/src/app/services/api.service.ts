import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import { LocalStorageService } from './local-storage.service';

type CallType = 'get' | 'put' | 'post' | 'delete';

type HttpClientRequestOptions = {
  headers?: HttpHeaders,
  params?: HttpParams,
}

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

	private makeApiCall(callType: CallType, fullUrl: string, requestData?: any, options: HttpClientRequestOptions = {}) : Observable<any> {
		if (!options.headers)
			options.headers = new HttpHeaders();

		options.headers = options.headers.set('Content-Type', 'application/json;charset=utf-8');

		switch (callType) {
			case 'get':
				return this.http.get(fullUrl, options);
			case 'put':
        return this.http.put(fullUrl, requestData, options);
      case 'post':
        return this.http.post(fullUrl, requestData, options);
      case 'delete':
        return this.http.delete(fullUrl, options);
		}
	}

	public makeAuthorizedApiCall(callType: CallType, relativeUrl: string, requestData?: any) : Observable<any> {
		const finalUrl = `${this.API_ROOT}${relativeUrl}`;
		const options = this.buildAuthorizationHeader();

		return this.makeApiCall(callType, finalUrl, requestData, options);
	}



}
