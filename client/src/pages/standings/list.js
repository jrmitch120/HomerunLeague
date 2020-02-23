import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';

@inject(Api)
export class List {

  _api;
  teams = [];
  year;
  
  constructor(api) {
    this._api = api;
    this.year = api.year;
  }

  activate(params) {
    // Load all pages for now...
    let loadData = (page) => {
      return this._api.getTeams(page).then(results => {

        this.teams = this.teams.concat(results.teams);

        if (results.meta.page < results.meta.totalPages)
          return loadData(results.meta.page + 1);
      });
    };

    return loadData();
  }
}


