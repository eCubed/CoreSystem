import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss']
})
export class ContactsComponent implements OnInit, OnDestroy {

	getContactsSubscription: Subscription;
	contactsResultSet: any;

  constructor(private apiService: ApiService) { 

  }

  ngOnInit() {

  	this.getContactsSubscription = this.apiService.makeAuthorizedApiCall("get", `contacts/search/*/1/200`).subscribe(res => {
  		this.contactsResultSet = res;
  	},
  	err => {
  	});
  }

  ngOnDestroy() {
  	if (this.getContactsSubscription != null)
  		this.getContactsSubscription.unsubscribe();
  }

}
