import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from './services/api';


@inject(EventAggregator, Api)
export class App {
  constructor(eventAggregator, api) {
    this.api = api;

    // Auto-scroll the window to the top after navigating to a new route. 
    eventAggregator.subscribe('router:navigation:complete', payload => window.scrollTo(0, 0));
  }

  configureRouter(config, router) {
    config.title = 'Homerun League';
    config.map([
      { route: ['', 'welcome'], name: 'welcome', moduleId: 'pages/welcome/index', nav: true, title: 'Welcome' },
      { route: 'teams/create', name: 'join', moduleId: 'pages/teams/create', nav: true, title: 'Join League' },
      { route: 'leaders', name: 'leaders', moduleId: 'pages/leaders/homeruns', nav: true, title: 'Leaders' },
      { route: 'standings', name: 'standings', moduleId: 'pages/standings/list', nav: true, title: 'Standings' },
      { route: 'activity', name: 'activity', moduleId: 'pages/activity/activity', nav: true, title: 'Activity' },
    ]);

    this.router = router;
  }
}