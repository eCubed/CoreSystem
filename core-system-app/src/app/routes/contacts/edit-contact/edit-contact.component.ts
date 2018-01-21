import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

import { ApiService } from '../../../services/api.service';

import { SaveContactViewModel } from '../models/savecontactviewmodel';

import { fadeInAnimation } from '../../../animations/animations';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.scss'],
  animations: [fadeInAnimation],
  host: { '[@fadeInAnimation]': ''}
})
export class EditContactComponent implements OnInit, OnDestroy {

	contact: SaveContactViewModel;
  saveContactSubscription: Subscription;
  getContactSubscription: Subscription;
  routerSubscription: Subscription;

  constructor(private apiService: ApiService,
  						private activatedRoute: ActivatedRoute) { }


  ngOnInit() {
  	this.contact = new SaveContactViewModel();

  	this.routerSubscription = this.activatedRoute.params.subscribe(params => {
  		var id = params['id'] || 0;

  		this.getContactSubscription = this.apiService.makeAuthorizedApiCall("get", `contacts/${id}`).subscribe(res => {
  			this.contact = res.data;
  		},
  		error => {

  		});
  	});

  }

  ngOnDestroy() {
    if (this.saveContactSubscription != null)
      this.saveContactSubscription.unsubscribe();

    if (this.getContactSubscription != null)
    	this.getContactSubscription.unsubscribe();

    if (this.routerSubscription != null)
    	this.routerSubscription.unsubscribe();
  }


  onSaveContactRequestHandle(contact: SaveContactViewModel) {    
    this.saveContactSubscription = this.apiService.makeAuthorizedApiCall("put", "contacts", contact).subscribe(res => {
      // this.contact.id = res.id;
      // Forward to the get version to proceed.
    }, error => {
      console.log(error.error);
    });    
  }

}
