import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { ApiService } from '../../../services/api.service';

import { SaveContactViewModel } from '../models/savecontactviewmodel';

import { fadeInAnimation } from '../../../animations/animations';

@Component({
  selector: 'app-create-contact',
  templateUrl: './create-contact.component.html',
  styleUrls: ['./create-contact.component.scss'],
  animations: [fadeInAnimation],
  host: { '[@fadeInAnimation]': ''}
})
export class CreateContactComponent implements OnInit, OnDestroy {

	contact: SaveContactViewModel;
  saveContactSubscription: Subscription;

  constructor(private apiService: ApiService) { }

  ngOnInit() {
  	this.contact = new SaveContactViewModel();
  }

  ngOnDestroy() {
    if (this.saveContactSubscription != null)
      this.saveContactSubscription.unsubscribe();
  }

  onSaveContactRequestHandle(contact: SaveContactViewModel) {
    if (contact.id == 0) {
      this.saveContactSubscription = this.apiService.makeAuthorizedApiCall("post", "contacts", contact).subscribe(res => {
        this.contact.id = res.id;
        // Forward to the get version to proceed.
      }, error => {
        console.log(error.error);
      });
    }
  }

}
