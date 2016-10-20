import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from '../../services/api';
import $ from 'bootstrap';
import moment from 'moment';

@inject(EventAggregator, Api)
export class HomeRuns {

  _api;
  _ea;
  _subscriber;

  leaders;

  constructor(eventAggregator, api) {
    this._api = api;
    this._ea = eventAggregator;
  }

  activate(params) {
    return this._getData();
  }

  attached() {
    // Enable the tooltips
    $('[data-toggle="tooltip"]').tooltip();
    
    // Subscript to team update notifications
    this._subscriber = this._ea.subscribe('TeamUpdate', () => { return this._getData() });
  }

  detached() {
    this._subscriber.dispose(); // cleanup the subscription
  }

  _getData() {
    this.leaders = [];

    // Load all pages for now...
    let getLeaders = (page) => {
      return this._api.getLeaders(page).then(results => {

        this.leaders = this.leaders.concat(results.leaders);

        if (results.meta.page < results.meta.totalPages)
          return getLeaders(results.meta.page + 1);
      });
    };

    return getLeaders().then(() => this._initHistory());
  }

  _initHistory() {
    for (let leader of this.leaders) {
      leader.history = { "recentHr": [], "loaded": false };
    }
  }

  _loadHistory(player, gamelogs) {
    if (player.history.loaded)
      return;

    var now = moment()

    for (let gamelog of gamelogs) {
      let gameDate = moment(gamelog.gameDate);

      if (player.history.recentHr.length < 5 && gamelog.hr > 0) {
        let location = gamelog.homeAway === 'A' ? 'at' : 'vs'
        player.history.recentHr.push({ "date": gameDate.format('MM/DD'), "hr": gamelog.hr, "location": location, "opp": gamelog.opponent });
      }
    }

    player.history.loaded = true;
  }

  toggleHistory(player) {
    var spinner = $(`#spinner-${player.playerId}`);
    var history = $(`#history-${player.playerId}`);

    if(!history.is(':visible'))
      spinner.show();

    this._api.getGameLogsForPlayer(player.playerId).then(results => {
      this._loadHistory(player, results.gameLogs);
      spinner.hide();
      history.slideToggle('fast', function () { });
    });
  }
}


