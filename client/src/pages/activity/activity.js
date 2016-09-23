import {inject} from 'aurelia-framework';
import {Api} from '../../services/api';
import $ from 'bootstrap';

@inject(Api)
export class Activity {
  constructor(api) {
    this.api = api;
    this.recent = [];
  }

  activate(params) {
    // Load all pages for now...
    let loadRecent = (page) => {
      return this.api.getRecentHr(page).then(results => {

        this.recent = this.recent.concat(results.RecentHrs);
        
        if (results.Meta.Page < results.Meta.TotalPages)
          return loadRecent(results.Meta.Page + 1);
      });
    }

    return loadRecent();
  }
}