import {inject} from 'aurelia-framework';
import {Api} from '../../services/api';

@inject(Api)
export class Welcome {
	year;

	constructor(api) {
		this.year = api.year;
	}
}
