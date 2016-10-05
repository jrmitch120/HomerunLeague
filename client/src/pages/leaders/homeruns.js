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
    this._subscriber = this._ea.subscribe('TeamUpdate', () => { return this.getData() });
  }

  detached() {
    this._subscriber.dispose(); // cleanup the subscription
  }

  _getData() {
    this.leaders = [];

    // Load all pages for now...
    let getLeaders = (page) => {
      return this._api.getLeaders(page).then(results => {

        this.leaders = this.leaders.concat(results.Leaders);

        if (results.Meta.Page < results.Meta.TotalPages)
          return getLeaders(results.Meta.Page + 1);
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
      let gameDate = moment(gamelog.GameDate);

      if (player.history.recentHr.length < 5 && gamelog.Hr > 0) {
        let location = gamelog.HomeAway === 'A' ? 'at' : 'vs'
        player.history.recentHr.push({ "date": gameDate.format('MM/DD'), "hr": gamelog.Hr, "location": location, "opp": gamelog.Opponent });
      }
    }

    player.history.loaded = true;
  }

  toggleHistory(player) {
    var spinner = $(`#spinner-${player.PlayerId}`);
    var history = $(`#history-${player.PlayerId}`);

    if(!history.is(':visible'))
      spinner.show();

    this._api.getGameLogsForPlayer(player.PlayerId).then(results => {
      this._loadHistory(player, results.GameLogs);
      spinner.hide();
      history.slideToggle('fast', function () { });
    });
  }
}


