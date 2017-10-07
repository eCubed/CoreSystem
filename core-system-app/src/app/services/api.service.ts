import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class ApiService {

	public readonly apiRoot: string = 'http://localhost:xxxxx/api/';
	private readonly tokenEndpoint: string = 'http://localhost:yyyy/token';

  constructor(private localStorageService: LocalStorageService,
              private http: HttpClient) { 
  }

}
