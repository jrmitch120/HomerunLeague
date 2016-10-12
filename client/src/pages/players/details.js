import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';

@inject(Api)
export class Details {

  _api;

  player;
  dynamicDoughnutData;
  simpleLineData;

  constructor(api) {
    this._api = api;

    this.resetPieData();
    this.resetLineData();
  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;
    this.routeConfig.navModel.setTitle('Testing changing title');

    console.info(`Player Details Id ${params.id}`)

    return this._api.getPlayer(params.id).then(response => {
      this.player = response.Player
    });
  }

  resetPieData() {
    this.dynamicDoughnutData = {
      labels: ["Red", "Green", "Yellow"],
      datasets: [
        {
          data: [300, 50, 100],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ],
          hoverBackgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ]
        }]
    };
  }

  resetLineData() {
    this.simpleLineData = {
      labels: ["April", "May", "June", "July", "August", "September", "October"],
      datasets: [
        {
          label: "Healthy People",
          backgroundColor: "rgba(220,220,220,0.2)",
          borderColor: "rgba(220,220,220,1)",
          pointColor: "rgba(220,220,220,1)",
          pointStrokeColor: "#fff",
          pointHighlightFill: "#fff",
          pointHighlightStroke: "rgba(220,220,220,1)",
          data: [65, 59, 80, 81, 56, 55, 40]
        },
        {
          label: "Ill People",
          backgroundColor: "rgba(151,187,205,0.2)",
          borderColor: "rgba(151,187,205,1)",
          pointColor: "rgba(151,187,205,1)",
          pointStrokeColor: "#fff",
          pointHighlightFill: "#fff",
          pointHighlightStroke: "rgba(151,187,205,1)",
          data: [28, 48, 40, 19, 86, 27, 90]
        }
      ]
    };
  }

  addEntry() {
    this.dynamicDoughnutData.labels.push("New Colour");
    this.dynamicDoughnutData.datasets[0].data.push(50);
    this.dynamicDoughnutData.datasets[0].backgroundColor.push("#B4FD5C");
  };

}