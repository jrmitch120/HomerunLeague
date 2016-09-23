import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
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
						return response.json();
					}
				})
		});

		this.http = http;
	}

	// Create a new Team
	createTeam(team) {
		return this.http.fetch(`seasons/${this.year}/teams`, {
			method: 'post',
			body: json(team)
		});
	}

	// Get Administrative Event History
	getEvents(action, page = 1) {
		return this.http.fetch(`admin/events?page=${page}&action=${action}`)
	}

	// Get Divisions
	getDivisions() {
		return this.http.fetch(`seasons/${this.year}/divisions`);
	}

	// Get GameLogs For Player
	getGameLogsForPlayer(playerId, year = this.year, page = 1) {
		return this.http.fetch(`players/${playerId}/gamelogs?year=${year}&page=${page}`);
	}

	// Get Leaders
	getLeaders(page = 1) {
		return this.http.fetch(`seasons/${this.year}/leaders?page=${page}`);
	}

	// Get Recent HR
	getRecentHr(page = 1) {
		return this.http.fetch(`seasons/${this.year}/recent?page=${page}`);
	}

	// Get Settings
	getSettings() {
		return this.http.fetch(`admin/settings`);
	}

	// Get Teams
	getTeams(page = 1) {
		return this.http.fetch(`seasons/${this.year}/teams?page=${page}`);
	}
}