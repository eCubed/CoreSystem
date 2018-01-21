import { Component, OnInit } from '@angular/core';

import { fadeInAnimation } from '../../animations/animations';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
  animations: [fadeInAnimation],
  host: { '[@fadeInAnimation]': ''}
})
export class NotFoundComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
