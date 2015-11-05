import {inject} from 'aurelia-framework';
import {Configure} from 'aurelia-configuration';
import {HttpClient} from 'aurelia-http-client';

@inject(Configure, HttpClient)
export class DivisionData {
	 constructor(config, http) {
        this.config = config;
		this.http = http.configure(x => { x.withHeader('Content-Type', 'application/json');});
		this.baseUrl = this.config.get('api.endpoint');
		this.year = this.config.get('year');
		
		console.info(this.config);
		console.info('BaseUrl:' + this.baseUrl);
	 }
	 
	 getDivisions() {
	 	return this.http.get(this.baseUrl + `${this.year}/divisions`).then(resp => {
			  console.log(resp.content);
		      return resp.content;
		  });
	 }
	 
	 /*
	 getPlayer(id) {
		  return this.http.get(this.baseUrl + `players/${id}`).then(resp => {
			  console.log(resp.content);
		      return resp.content;
		  });
	  }
	  */
}