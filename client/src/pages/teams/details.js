import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';

@inject(Api)
export class Create {

  _api;
  team;

  constructor(api) {
    this._api = api;
  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this._api.getTeam(params.id).then(response => {
      if (response.team) {
        this.team = response.team;
        this.routeConfig.navModel.setTitle(this.team.name);
        console.info(this.team);
      }
      else
        this.routeConfig.navModel.setTitle('Team Not Found');
    });
  }
}