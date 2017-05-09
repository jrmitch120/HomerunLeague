import {bindable, inject} from 'aurelia-framework'
import moment from 'moment';

@inject()
export class NavBar {
	@bindable router = null;
	@bindable lastTeamUpdate = null;

	constructor() {
		this.ticker = null;
		this.updateStatus = '';
	}

	attached() {
		this.setUpdateStatus();
		this.ticker = setInterval(() => this.setUpdateStatus(), 60000);
	}

	detached() {
		if(this.ticker !== null)
			clearInterval(this.ticker);
	}
	
	setUpdateStatus() {
		if (this.lastTeamUpdate === null)
			this.updateStatus = '';

		if (this.lastTeamUpdate === undefined)
			this.updateStatus =  'Stat update running now'

		let update = moment(this.lastTeamUpdate).toDate()

		console.info('Last update: *' + update + '*');

		this.updateStatus =  'Stats updated ' +
			moment(moment.utc([update.getFullYear(),
				update.getMonth(),
				update.getDate(),
				update.getHours(),
				update.getMinutes(),
				update.getSeconds()]).toDate()).fromNow();
	}
}	
