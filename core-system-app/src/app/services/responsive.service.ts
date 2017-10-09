import { Injectable, OnDestroy } from '@angular/core';
import { Observable, Subscription, Subject } from 'rxjs';

@Injectable()
export class ResponsiveService implements OnDestroy {
	/*
		Media queries in CSS are great, and they already do the work of physically changing the looks and placement of
		elements when the window resizes. However, we may want to find out at least which of small, medium, and large the
		current viewport is, and plug it into the logic of some of our components. We will need this service to detect
		the window's size every time it changes
	*/

	smallToMediumThreshhold: number = 400;
	mediumToLargeThreshhold: number = 800;

	currentWidth: number;
	currentWidthIdentifier$: Subject<string> = new Subject<string>();
  currentWidthIdentifierValue: string;

	loadSubscription:Subscription;
	resizeSubscription:Subscription;

  constructor() {
    this.currentWidth = window.innerWidth;
    this.determinCurrentWidthIdentifier();

  	this.loadSubscription = Observable.fromEvent(window, "load").subscribe(e => {
  		this.currentWidth = window.innerWidth;
  		this.determinCurrentWidthIdentifier();
  	});

  	this.resizeSubscription = Observable.fromEvent(window, "resize").subscribe(e => {
  		this.currentWidth = window.innerWidth;
  		this.determinCurrentWidthIdentifier();
  	});
  }

  ngOnDestroy() {
  	if (this.loadSubscription != null) 
  		this.loadSubscription.unsubscribe();
  	if (this.resizeSubscription != null)
  		this.resizeSubscription.unsubscribe();
  }

  determinCurrentWidthIdentifier() {
  	if (this.currentWidth <= this.smallToMediumThreshhold) {
      this.currentWidthIdentifierValue = "small";
  		this.currentWidthIdentifier$.next("small");
    }
  	else if ((this.currentWidth > this.smallToMediumThreshhold) &&
  		(this.currentWidth <= this.mediumToLargeThreshhold)) {
      this.currentWidthIdentifierValue = "medium";
      this.currentWidthIdentifier$.next("medium");
    }
  	else{
      this.currentWidthIdentifierValue = "large";
  		this.currentWidthIdentifier$.next("large");
    }
  }

  getWidthIdentifierChanged(): Observable<string> {
  	return this.currentWidthIdentifier$.asObservable().distinctUntilChanged();
  }

  getCurrentWidthIdentifier() {
    if (this.currentWidthIdentifier$ == null) {
      this.determinCurrentWidthIdentifier();
    }
    return this.currentWidthIdentifierValue;
  }

}
