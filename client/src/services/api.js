import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'whatwg-fetch'; // polyfil for fetch
import environment from '../environment'

@inject(environment, HttpClient)
export class Api {
	isRequesting = false;

	constructor(environment, http) {

		// Year we're interested in
		this.year = environment.year;

		// Configure fetch client
		http.configure(config => {
			config
				.withBaseUrl(environment.api)
				.withDefaults({
					headers: {
						'Accept': 'application/json'
					}
				})
				.withInterceptor({
					request(request) {
						this.isRequesting = true;
						return request;
					},
					response(response) {
						this.isRequesting = false;
						return response;
					}
				})
		});

		this.http = http;
	}

	// Get Divisions
	getDivisions() {
		return this.http.fetch(`seasons/${this.year}/divisions`)
			.then(response => response.json())
			.then(data => { return data; });
	}

	// Get Leaders
	getLeaders(page = 1) {
		return this.http.fetch(`seasons/${this.year}/leaders?page=${page}`)
			.then(response => response.json())
			.then(data => { return data; });
	}

	// Get GameLogs For Player
	getGameLogsForPlayer(playerId, year=this.year, page = 1) {
		return this.http.fetch(`players/${playerId}/gamelogs?year=${year}&page=${page}`)
			.then(response => response.json())
			.then(data => { return data; });
	}

	// Get Teams
	getTeams(page = 1) {
		return this.http.fetch(`seasons/${this.year}/teams?page=${page}`)
			.then(response => response.json())
			.then(data => { return data; });
	}
}