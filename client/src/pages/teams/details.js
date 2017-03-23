import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';

@inject(Api)
export class Create {
  _api;

  team;
  registrationOpen;

  constructor(api) {
    this._api = api;
  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    // See if registration is open, skip team load if we're currently in open enrollment.
    return this._api.getSettings().then(response => {
      this.registrationOpen = response.registrationOpen;

      // Only fetch team information if registration is closed
      if (!this.registrationOpen) {
        return this._api.getTeam(params.id).then(response => {
          if (response.team) {
            this.team = response.team;
            this.routeConfig.navModel.setTitle(this.team.name);
          }
          else
            this.routeConfig.navModel.setTitle('Team Not Found');
        });
      }
    });
  }
}