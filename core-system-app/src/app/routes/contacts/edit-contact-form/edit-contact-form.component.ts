import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { SaveContactViewModel } from '../models/savecontactviewmodel';

import { fadeInAnimation } from '../../../animations/animations';

@Component({
  selector: 'app-edit-contact-form',
  templateUrl: './edit-contact-form.component.html',
  styleUrls: ['./edit-contact-form.component.scss'],  
  animations: [fadeInAnimation]
})
export class EditContactFormComponent implements OnInit {

	@Input("contact") contact: SaveContactViewModel;
	@Output("onSaveContactRequest") onSaveContactRequest: EventEmitter<SaveContactViewModel> =
		new EventEmitter<SaveContactViewModel>();

  constructor() { }

  ngOnInit() {

  }

  onSubmitClicked() {
  	this.onSaveContactRequest.emit(this.contact);
  }

}
