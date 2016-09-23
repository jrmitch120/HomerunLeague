import {bindable} from 'aurelia-framework'
import moment from 'moment';

export class NavBar {
	@bindable router = null;
	@bindable api = null;

	constructor() {
		this.updateStatus = '';
  }

	attached() {
		this.getUpdateStatus();
		setInterval(() => this.getUpdateStatus(), 60000);
	}

	getUpdateStatus() {
		this.api.getEvents('TeamUpdate').then(results => {
			if (results != null) {
				this.setUpdateStatus(results.LeagueEvents[0].Completed);
			}
		});
	}

	setUpdateStatus(completed) {
		if(completed === undefined) {
			this.updateStatus = 'Stat update running now'
			return;
		}
		
		completed = moment(completed).toDate()

		this.updateStatus = 'Stats updated ' +
			moment(moment.utc([completed.getFullYear(),
				completed.getMonth(),
				completed.getDate(),
				completed.getHours(),
				completed.getMinutes(),
				completed.getSeconds()]).toDate()).fromNow();//.format('M/D, h:mm a');
	}
}