import {Api} from './services/api';

export class App {
  configureRouter(config, router){
    config.title = 'Homerun League';
    config.map([
      { route: ['','welcome'], name: 'welcome',   moduleId: 'pages/welcome/index',              nav: true, title: 'Welcome' },
      { route: 'teams/create', name: 'join',      moduleId: 'pages/teams/create',   nav: true, title: 'Join League'   },
      { route: 'leaders',    name: 'leaders', moduleId: 'pages/leaders/homeruns', nav: true, title: 'Leaders'   },
      { route: 'standings',    name: 'standings', moduleId: 'pages/standings/list', nav: true, title: 'Standings'   },
    ]);
    
    this.router = router;
  }
}