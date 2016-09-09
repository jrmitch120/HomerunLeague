import {inject} from 'aurelia-framework';
import {Api} from '../../services/api';
import $ from 'bootstrap';
import moment from 'moment';

@inject(Api)
export class HomeRuns {
  constructor(api) {
    this.api = api;
    this.leaders = [];
  }

  activate(params) {
    // Load all pages for now...
    let loadLeaders = (page) => {
      return this.api.getLeaders(page).then(results => {

        this.leaders = this.leaders.concat(results.Leaders);

        if (results.Meta.Page < results.Meta.TotalPages)
          return loadLeaders(results.Meta.Page + 1);
      });
    };

    return loadLeaders().then(() => this.initHistory());
  }

  attached() {
    // Enable the tooltips
    $('[data-toggle="tooltip"]').tooltip()
  }

  initHistory() {
    for (let leader of this.leaders) {
      leader.history = { "recentHr": [], "loaded": false };
    }
  }

  loadHistory(player, gamelogs) {
    if (player.history.loaded)
      return;

    var now = moment()
    
    for (let gamelog of gamelogs) {
      let gameDate = new moment(gamelog.GameDate);

      if(player.history.recentHr.length < 5 && gamelog.Hr > 0)
      {
        let location = gamelog.HomeAway === 'A' ? 'at' : 'vs'
        player.history.recentHr.push({"date": gameDate.format('MM/DD'), "hr": gamelog.Hr, "location": location, "opp": gamelog.Opponent});
      }
    }

    player.history.loaded = true;
  }

  toggleHistory(player) {
    if (!(player.PlayerId in history)) {
      this.api.getGameLogsForPlayer(player.PlayerId).then(results => {
        this.loadHistory(player, results.GameLogs);
        $(`#history-${player.PlayerId}`).slideToggle('fast', function () { });
      });
    }
  }
}


