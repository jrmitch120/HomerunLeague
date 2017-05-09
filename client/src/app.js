import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Api } from './services/api';

@inject(EventAggregator, Api)
export class App {

  _api;
  _ea;

  lastTeamUpdate = null;

  constructor(eventAggregator, api) {
    this._ea = eventAggregator;
    this._api = api;

    // Auto-scroll the window to the top after navigating to a new route. 
    eventAggregator.subscribe('router:navigation:complete', payload => window.scrollTo(0, 0));
  }

  activate() {
    // Set into motion stat check every 60 seconds.

    setInterval(() => this._getUpdateStatus(), 60000);

    // Have Aurelia wait until we get the settings
    return Promise.all([
      this._getUpdateStatus(),
      this._api.getSettings().then(settings => { this._api.year = settings.baseballYear })
    ]);
  }

  // Configure router
  configureRouter(config, router) {
    config.title = 'Homerun League';
    config.map([
      { route: ['', 'welcome'], name: 'welcome', moduleId: 'pages/welcome/index', nav: true, title: 'Welcome' },
      { route: 'teams/create', name: 'join', moduleId: 'pages/teams/create', nav: true, title: 'Join League' },
      { route: 'leaders', name: 'leaders', moduleId: 'pages/leaders/homeruns', nav: true, title: 'Leaders' },
      { route: 'standings', name: 'standings', moduleId: 'pages/standings/list', nav: true, title: 'Standings' },
      { route: 'activity', name: 'activity', moduleId: 'pages/activity/activity', nav: true, title: 'Activity' },
      { route: 'teams/:id', name: 'team', moduleId: 'pages/teams/details', nav: false, title: 'Team Information' },
      { route: 'players/:id', name: 'player', moduleId: 'pages/players/details', nav: false, title: 'Player Infomration' },
    ]);

    this.router = router;
  }

  // Called every 60 seconds to check for + notify of TeamStat updates
  _getUpdateStatus() {
    return this._api.getEvents('TeamUpdate').then(results => {

      if (this.lastTeamUpdate !== null && results.leagueEvents[0].completed !== this.lastTeamUpdate) {
        this._ea.publish('TeamUpdate');
        console.info('TeamUpdate event published');
      }

      this.lastTeamUpdate = results.leagueEvents[0].completed;
    });
  }
}