import {inject} from 'aurelia-framework';
import {Api} from '../../services/api';

@inject(Api)
export class List {

  _api;
  teams = [];

  constructor(api) {
    this._api = api;
  }

  activate(params) {
    // Load all pages for now...
    let loadData = (page) => {
      return this._api.getTeams(page).then(results => {
        
        this.teams = this.teams.concat(results.Teams);

        if (results.Meta.Page < results.Meta.TotalPages)
          return loadData(results.Meta.Page + 1);
      });
    };

    return loadData();
  }
}


