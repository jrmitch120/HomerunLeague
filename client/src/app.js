export class App {
  configureRouter(config, router){
    config.title = 'Homerun League';
    config.map([
      { route: ['','welcome'], name: 'welcome',   moduleId: 'welcome',        nav: true, title: 'Welcome' },
      { route: 'join',         name: 'join',      moduleId: 'teams/create', nav: true, title: 'Join League'   },
      { route: 'standings',    name: 'standings', moduleId: 'standings/list', nav: true, title: 'Standings'   },
    ]);
    
    this.router = router;
  }
}
