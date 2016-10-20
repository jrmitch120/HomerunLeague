import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';
import moment from 'moment';;

@inject(Api)
export class Details {

  _api;

  _chartDataCache = {
    gamelogs: [],
    hrTotalsByMonth: [],
    abTotalsByMonth: []
  };

  player;
  statsYear;
  radarHrData;
  radarAbData;

  constructor(api) {
    this._api = api;

    this._setupRadarData();
  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this._api.getPlayer(params.id).then(response => {
      if (response.player) {
        this.player = response.player;
        this.routeConfig.navModel.setTitle(this.player.displayName);

        if (this.player.playerTotals.length > 0) {
          // Set the first playtotal as active
          this.player.playerTotals[0].isSelected = true;

          // Set statsYear to first playerTotal
          this.statsYear = this.player.playerTotals[0].Year;

          // Bind charts to statsYear
          return this._bindChartDatasets(this.statsYear);
        }
      }
      else
        this.routeConfig.navModel.setTitle('Player Not Found');
    });
  }

  _getGamelogs(year) {
    if (this._chartDataCache.gamelogs[year] == undefined) {
      return this._api.getGameLogsForPlayer(this.player.id, year).then(response => {
        console.info('Fetching gamelogs from server...');
        this._chartDataCache.gamelogs[year] = response.gameLogs;
        return response.gameLogs;
      });
    }
    else {
      let gamelogs = this._chartDataCache.gamelogs[year];

      return new Promise(function (resolve) {
        console.info('Gamelogs cached...');
        resolve(gamelogs);
      });
    }
  }

  _bindChartDatasets(year) {
    return this._getGamelogs(year).then(gamelogs => {

      // Calculate yearly gamelog totals if no cached.
      if (this._chartDataCache.hrTotalsByMonth[year] == undefined) {

        this._chartDataCache.hrTotalsByMonth[year] = [];
        this._chartDataCache.abTotalsByMonth[year] = [];

        // Reset monthly totals for March - October.
        for (var i = 2; i < 10; i++) {
          console.info(i);
          this._chartDataCache.hrTotalsByMonth[year][i] = 0;
          this._chartDataCache.abTotalsByMonth[year][i] = 0;
        }

        gamelogs.forEach(gamelog => {
          let month = moment(gamelog.gameDate).month();
          console.info(`Month: ${month}`);
          this._chartDataCache.hrTotalsByMonth[year][month] += gamelog.hr;
          this._chartDataCache.abTotalsByMonth[year][month] += gamelog.ab;
        });
      }

      // Set chart dataset data
      this.radarHrData.datasets[0].data.length = 0;
      this.radarAbData.datasets[0].data.length = 0;
      this._chartDataCache.hrTotalsByMonth[year].forEach(stat => { this.radarHrData.datasets[0].data.push(stat); });
      this._chartDataCache.abTotalsByMonth[year].forEach(stat => { this.radarAbData.datasets[0].data.push(stat); });

      console.info(this.radarHrData.datasets[0].data);
    });
  }

  _setupRadarData() {
    this.radarHrData = {
      labels: ['March', 'April', 'May', 'June', 'July', 'August', 'September', 'October'],
      datasets: [{
        label: 'Home Runs',
        backgroundColor: 'rgba(255,99,132,0.2)',
        borderColor: 'rgba(255,99,132,1)',
        pointBackgroundColor: 'rgba(255,99,132,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(255,99,132,1)',
        data: []
      }]
    };

    this.radarAbData = {
      labels: ['March', 'April', 'May', 'June', 'July', 'August', 'September', 'October'],
      datasets: [{
        label: 'At Bats',
        backgroundColor: 'rgba(255,99,132,0.2)',
        borderColor: 'rgba(255,99,132,1)',
        pointBackgroundColor: 'rgba(255,99,132,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(255,99,132,1)',
        data: []
      }]
    };
  }

  bindCharts(playerTotal) {
    // Row highlighting
    this.player.playerTotals.forEach(x => x.isSelected = false);
    this.statsYear = playerTotal.year;

    playerTotal.isSelected = true;

    // Bind the chart data
    return this._bindChartDatasets(this.statsYear);
  }
}