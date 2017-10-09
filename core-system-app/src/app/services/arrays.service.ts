import { Injectable } from '@angular/core';

@Injectable()
export class ArraysService {

    delete(item: Object, array: Object[])
	{
		var index = array.indexOf(item);
		if (index >= 0)
			array.splice(index, 1);
	}

	add(item: Object, array: Object[])
	{
		array.push(item);
	}

	clear(array: Object[])
	{
		array.splice(0, array.length);
	}

	insertAfter(index: number, item: Object, array: Object[])
 	{
 		if ((index >= 0) && (index < array.length))
 		{
			array.splice(index, 0, item);
 		}
 	}

 	insertBefore(index: number, item: Object, array: Object[])
 	{
		if ((index >= 1) && (index < array.length)) {
			array.splice(index-1, 0, item);
	  	}
 	}

 	anyOfFirstIsInSecond(firstArray: Object[], secondArray: Object[])
 	{
		for (var i = 0; i < firstArray.length; i++){
			if (secondArray.indexOf(firstArray[i]) > -1)
				return true;
		}

		return false;
 	}

	allOfFirstIsInSecond(firstArray: Object[], secondArray: Object[])
	{
		for (var i = 0; i < firstArray.length; i++) {
			if (secondArray.indexOf(firstArray[i]) == -1)
				return false;
		}

		return true;
	}

	canMoveUp(item:Object, array:Object[]):boolean{
		return (array.indexOf(item) > 0);
	}

	canMoveDown(item:Object, array:Object[]):boolean{
		return (array.indexOf(item) < array.length - 1);
	}

	moveUp(item:Object, array:Object[]){
    if(this.canMoveUp(item, array)){
			var index = array.indexOf(item);
			this.delete(item, array);
			this.insertAfter(index-1, item, array);
		}
	}

	moveDown(item:Object, array:Object[]){
		if(this.canMoveDown(item, array)){
			var index = array.indexOf(item);
			this.delete(item, array);
			if (index == array.length - 1)
				array.push(item);
			else
				this.insertAfter(index+1, item, array);
		}
	}
}
