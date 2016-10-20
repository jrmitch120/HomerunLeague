define('app',['exports', 'aurelia-framework', 'aurelia-event-aggregator', './services/api'], function (exports, _aureliaFramework, _aureliaEventAggregator, _api) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.App = undefined;

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _dec, _class;

  var App = exports.App = (_dec = (0, _aureliaFramework.inject)(_aureliaEventAggregator.EventAggregator, _api.Api), _dec(_class = function () {
    function App(eventAggregator, api) {
      _classCallCheck(this, App);

      this.lastTeamUpdate = null;

      this._ea = eventAggregator;
      this._api = api;

      eventAggregator.subscribe('router:navigation:complete', function (payload) {
        return window.scrollTo(0, 0);
      });
    }

    App.prototype.activate = function activate() {
      var _this = this;

      this._getUpdateStatus();
      setInterval(function () {
        return _this._getUpdateStatus();
      }, 60000);

      return this._api.getSettings().then(function (settings) {
        _this._api.year = settings.baseballYear;
      });
    };

    App.prototype.configureRouter = function configureRouter(config, router) {
      config.title = 'Homerun League';
      config.map([{ route: ['', 'welcome'], name: 'welcome', moduleId: 'pages/welcome/index', nav: true, title: 'Welcome' }, { route: 'teams/create', name: 'join', moduleId: 'pages/teams/create', nav: true, title: 'Join League' }, { route: 'leaders', name: 'leaders', moduleId: 'pages/leaders/homeruns', nav: true, title: 'Leaders' }, { route: 'standings', name: 'standings', moduleId: 'pages/standings/list', nav: true, title: 'Standings' }, { route: 'activity', name: 'activity', moduleId: 'pages/activity/activity', nav: true, title: 'Activity' }, { route: 'players/:id', name: 'player', moduleId: 'pages/players/details', nav: false, title: 'Player Infomration' }]);

      this.router = router;
    };

    App.prototype._getUpdateStatus = function _getUpdateStatus() {
      var _this2 = this;

      return this._api.getEvents('TeamUpdate').then(function (results) {

        if (_this2.lastTeamUpdate !== null && results.leagueEvents[0].completed !== _this2.lastTeamUpdate) {
          _this2._ea.publish('TeamUpdate');
          console.info('TeamUpdate event published');
        }

        _this2.lastTeamUpdate = results.leagueEvents[0].completed;
      });
    };

    return App;
  }()) || _class);
});
define('environment',['exports'], function (exports) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.default = {
    debug: true,
    testing: true,
    api: 'http://192.168.11.180:9001/api/'
  };
});
define('main',['exports', './environment'], function (exports, _environment) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.configure = configure;

  var _environment2 = _interopRequireDefault(_environment);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  Promise.config({
    warnings: {
      wForgottenReturn: false
    }
  });

  function configure(aurelia) {
    aurelia.use.standardConfiguration().feature('resources').plugin('aurelia-chart');

    if (_environment2.default.debug) {
      aurelia.use.developmentLogging();
    }

    if (_environment2.default.testing) {
      aurelia.use.plugin('aurelia-testing');
    }

    aurelia.start().then(function () {
      return aurelia.setRoot();
    });
  }
});
define('resources/index',['exports'], function (exports) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.configure = configure;
  function configure(config) {
    config.globalResources(['./elements/nav-bar', './elements/loading']);
  }
});
define('services/api',['exports', 'aurelia-framework', 'aurelia-fetch-client', '../environment', 'whatwg-fetch'], function (exports, _aureliaFramework, _aureliaFetchClient, _environment) {
	'use strict';

	Object.defineProperty(exports, "__esModule", {
		value: true
	});
	exports.Api = undefined;

	var _environment2 = _interopRequireDefault(_environment);

	function _interopRequireDefault(obj) {
		return obj && obj.__esModule ? obj : {
			default: obj
		};
	}

	function _classCallCheck(instance, Constructor) {
		if (!(instance instanceof Constructor)) {
			throw new TypeError("Cannot call a class as a function");
		}
	}

	var _dec, _class;

	var Api = exports.Api = (_dec = (0, _aureliaFramework.inject)(_environment2.default, _aureliaFetchClient.HttpClient), _dec(_class = function () {
		function Api(environment, http) {
			_classCallCheck(this, Api);

			this.isRequesting = false;

			this.year = new Date().getFullYear();

			http.configure(function (config) {
				config.withBaseUrl(environment.api).withDefaults({
					headers: {
						'Accept': 'application/json'
					}
				}).withInterceptor({
					request: function request(_request) {
						this.isRequesting = true;
						return _request;
					},
					response: function response(_response) {
						this.isRequesting = false;
						return _response.json();
					}
				});
			});

			this.http = http;
		}

		Api.prototype.createTeam = function createTeam(team) {
			return this.http.fetch('seasons/' + this.year + '/teams', {
				method: 'post',
				body: (0, _aureliaFetchClient.json)(team)
			});
		};

		Api.prototype.getEvents = function getEvents(action) {
			var page = arguments.length <= 1 || arguments[1] === undefined ? 1 : arguments[1];

			return this.http.fetch('admin/events?page=' + page + '&action=' + action);
		};

		Api.prototype.getDivisions = function getDivisions() {
			return this.http.fetch('seasons/' + this.year + '/divisions');
		};

		Api.prototype.getGameLogsForPlayer = function getGameLogsForPlayer(playerId) {
			var year = arguments.length <= 1 || arguments[1] === undefined ? this.year : arguments[1];
			var page = arguments.length <= 2 || arguments[2] === undefined ? 1 : arguments[2];

			return this.http.fetch('players/' + playerId + '/gamelogs?year=' + year + '&page=' + page);
		};

		Api.prototype.getLeaders = function getLeaders() {
			var page = arguments.length <= 0 || arguments[0] === undefined ? 1 : arguments[0];

			return this.http.fetch('seasons/' + this.year + '/leaders?page=' + page);
		};

		Api.prototype.getRecentHr = function getRecentHr() {
			var page = arguments.length <= 0 || arguments[0] === undefined ? 1 : arguments[0];

			return this.http.fetch('seasons/' + this.year + '/recent?page=' + page + '&start=8-8-2016');
		};

		Api.prototype.getPlayer = function getPlayer(playerId) {
			return this.http.fetch('players/' + playerId);
		};

		Api.prototype.getSettings = function getSettings() {
			return this.http.fetch('admin/settings');
		};

		Api.prototype.getTeams = function getTeams() {
			var page = arguments.length <= 0 || arguments[0] === undefined ? 1 : arguments[0];

			return this.http.fetch('seasons/' + this.year + '/teams?page=' + page);
		};

		return Api;
	}()) || _class);
});
define('pages/activity/activity',['exports', 'aurelia-framework', '../../services/api', 'bootstrap'], function (exports, _aureliaFramework, _api, _bootstrap) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.Activity = undefined;

  var _bootstrap2 = _interopRequireDefault(_bootstrap);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _dec, _class;

  var Activity = exports.Activity = (_dec = (0, _aureliaFramework.inject)(_api.Api), _dec(_class = function () {
    function Activity(api) {
      _classCallCheck(this, Activity);

      this.api = api;
      this.recent = [];
    }

    Activity.prototype.activate = function activate(params) {
      var _this = this;

      var loadRecent = function loadRecent(page) {
        return _this.api.getRecentHr(page).then(function (results) {

          _this.recent = _this.recent.concat(results.recentHrs);

          if (results.meta.page < results.meta.totalPages) return loadRecent(results.meta.page + 1);
        });
      };

      return loadRecent();
    };

    return Activity;
  }()) || _class);
});
define('pages/leaders/homeruns',['exports', 'aurelia-framework', 'aurelia-event-aggregator', '../../services/api', 'bootstrap', 'moment'], function (exports, _aureliaFramework, _aureliaEventAggregator, _api, _bootstrap, _moment) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.HomeRuns = undefined;

  var _bootstrap2 = _interopRequireDefault(_bootstrap);

  var _moment2 = _interopRequireDefault(_moment);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _dec, _class;

  var HomeRuns = exports.HomeRuns = (_dec = (0, _aureliaFramework.inject)(_aureliaEventAggregator.EventAggregator, _api.Api), _dec(_class = function () {
    function HomeRuns(eventAggregator, api) {
      _classCallCheck(this, HomeRuns);

      this._api = api;
      this._ea = eventAggregator;
    }

    HomeRuns.prototype.activate = function activate(params) {
      return this._getData();
    };

    HomeRuns.prototype.attached = function attached() {
      var _this = this;

      (0, _bootstrap2.default)('[data-toggle="tooltip"]').tooltip();

      this._subscriber = this._ea.subscribe('TeamUpdate', function () {
        return _this._getData();
      });
    };

    HomeRuns.prototype.detached = function detached() {
      this._subscriber.dispose();
    };

    HomeRuns.prototype._getData = function _getData() {
      var _this2 = this;

      this.leaders = [];

      var getLeaders = function getLeaders(page) {
        return _this2._api.getLeaders(page).then(function (results) {

          _this2.leaders = _this2.leaders.concat(results.leaders);

          if (results.meta.page < results.meta.totalPages) return getLeaders(results.meta.page + 1);
        });
      };

      return getLeaders().then(function () {
        return _this2._initHistory();
      });
    };

    HomeRuns.prototype._initHistory = function _initHistory() {
      for (var _iterator = this.leaders, _isArray = Array.isArray(_iterator), _i = 0, _iterator = _isArray ? _iterator : _iterator[Symbol.iterator]();;) {
        var _ref;

        if (_isArray) {
          if (_i >= _iterator.length) break;
          _ref = _iterator[_i++];
        } else {
          _i = _iterator.next();
          if (_i.done) break;
          _ref = _i.value;
        }

        var leader = _ref;

        leader.history = { "recentHr": [], "loaded": false };
      }
    };

    HomeRuns.prototype._loadHistory = function _loadHistory(player, gamelogs) {
      if (player.history.loaded) return;

      var now = (0, _moment2.default)();

      for (var _iterator2 = gamelogs, _isArray2 = Array.isArray(_iterator2), _i2 = 0, _iterator2 = _isArray2 ? _iterator2 : _iterator2[Symbol.iterator]();;) {
        var _ref2;

        if (_isArray2) {
          if (_i2 >= _iterator2.length) break;
          _ref2 = _iterator2[_i2++];
        } else {
          _i2 = _iterator2.next();
          if (_i2.done) break;
          _ref2 = _i2.value;
        }

        var gamelog = _ref2;

        var gameDate = (0, _moment2.default)(gamelog.gameDate);

        if (player.history.recentHr.length < 5 && gamelog.hr > 0) {
          var location = gamelog.homeAway === 'A' ? 'at' : 'vs';
          player.history.recentHr.push({ "date": gameDate.format('MM/DD'), "hr": gamelog.hr, "location": location, "opp": gamelog.opponent });
        }
      }

      player.history.loaded = true;
    };

    HomeRuns.prototype.toggleHistory = function toggleHistory(player) {
      var _this3 = this;

      var spinner = (0, _bootstrap2.default)('#spinner-' + player.playerId);
      var history = (0, _bootstrap2.default)('#history-' + player.playerId);

      if (!history.is(':visible')) spinner.show();

      this._api.getGameLogsForPlayer(player.playerId).then(function (results) {
        _this3._loadHistory(player, results.gameLogs);
        spinner.hide();
        history.slideToggle('fast', function () {});
      });
    };

    return HomeRuns;
  }()) || _class);
});
define('pages/players/details',['exports', 'aurelia-framework', '../../services/api', 'moment'], function (exports, _aureliaFramework, _api, _moment) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.Details = undefined;

  var _moment2 = _interopRequireDefault(_moment);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) {
    return typeof obj;
  } : function (obj) {
    return obj && typeof Symbol === "function" && obj.constructor === Symbol ? "symbol" : typeof obj;
  };

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _dec, _class;

  ;

  var Details = exports.Details = (_dec = (0, _aureliaFramework.inject)(_api.Api), _dec(_class = function () {
    function Details(api) {
      _classCallCheck(this, Details);

      this._chartDataCache = {
        gamelogs: [],
        hrTotalsByMonth: [],
        abTotalsByMonth: []
      };

      this._api = api;

      this._setupRadarData();
    }

    Details.prototype.activate = function activate(params, routeConfig) {
      var _this = this;

      this.routeConfig = routeConfig;

      return this._api.getPlayer(params.id).then(function (response) {
        if (response.player) {
          _this.player = response.player;
          _this.routeConfig.navModel.setTitle(_this.player.displayName);

          if (_this.player.playerTotals.length > 0) {
            _this.player.playerTotals[0].isSelected = true;

            _this.statsYear = _this.player.playerTotals[0].Year;

            return _this._bindChartDatasets(_this.statsYear);
          }
        } else _this.routeConfig.navModel.setTitle('Player Not Found');
      });
    };

    Details.prototype._getGamelogs = function _getGamelogs(year) {
      var _this2 = this;

      if (this._chartDataCache.gamelogs[year] == undefined) {
        return this._api.getGameLogsForPlayer(this.player.id, year).then(function (response) {
          console.info('Fetching gamelogs from server...');
          _this2._chartDataCache.gamelogs[year] = response.gameLogs;
          return response.gameLogs;
        });
      } else {
        var _ret = function () {
          var gamelogs = _this2._chartDataCache.gamelogs[year];

          return {
            v: new Promise(function (resolve) {
              console.info('Gamelogs cached...');
              resolve(gamelogs);
            })
          };
        }();

        if ((typeof _ret === 'undefined' ? 'undefined' : _typeof(_ret)) === "object") return _ret.v;
      }
    };

    Details.prototype._bindChartDatasets = function _bindChartDatasets(year) {
      var _this3 = this;

      return this._getGamelogs(year).then(function (gamelogs) {
        if (_this3._chartDataCache.hrTotalsByMonth[year] == undefined) {

          _this3._chartDataCache.hrTotalsByMonth[year] = [];
          _this3._chartDataCache.abTotalsByMonth[year] = [];

          for (var i = 2; i < 10; i++) {
            console.info(i);
            _this3._chartDataCache.hrTotalsByMonth[year][i] = 0;
            _this3._chartDataCache.abTotalsByMonth[year][i] = 0;
          }

          gamelogs.forEach(function (gamelog) {
            var month = (0, _moment2.default)(gamelog.gameDate).month();
            console.info('Month: ' + month);
            _this3._chartDataCache.hrTotalsByMonth[year][month] += gamelog.hr;
            _this3._chartDataCache.abTotalsByMonth[year][month] += gamelog.ab;
          });
        }

        _this3.radarHrData.datasets[0].data.length = 0;
        _this3.radarAbData.datasets[0].data.length = 0;
        _this3._chartDataCache.hrTotalsByMonth[year].forEach(function (stat) {
          _this3.radarHrData.datasets[0].data.push(stat);
        });
        _this3._chartDataCache.abTotalsByMonth[year].forEach(function (stat) {
          _this3.radarAbData.datasets[0].data.push(stat);
        });

        console.info(_this3.radarHrData.datasets[0].data);
      });
    };

    Details.prototype._setupRadarData = function _setupRadarData() {
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
    };

    Details.prototype.bindCharts = function bindCharts(playerTotal) {
      this.player.playerTotals.forEach(function (x) {
        return x.isSelected = false;
      });
      this.statsYear = playerTotal.year;

      playerTotal.isSelected = true;

      return this._bindChartDatasets(this.statsYear);
    };

    return Details;
  }()) || _class);
});
define('pages/standings/list',['exports', 'aurelia-framework', '../../services/api'], function (exports, _aureliaFramework, _api) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.List = undefined;

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _dec, _class;

  var List = exports.List = (_dec = (0, _aureliaFramework.inject)(_api.Api), _dec(_class = function () {
    function List(api) {
      _classCallCheck(this, List);

      this.teams = [];

      this._api = api;
    }

    List.prototype.activate = function activate(params) {
      var _this = this;

      var loadData = function loadData(page) {
        return _this._api.getTeams(page).then(function (results) {

          _this.teams = _this.teams.concat(results.teams);

          if (results.meta.page < results.meta.totalPages) return loadData(results.meta.page + 1);
        });
      };

      return loadData();
    };

    return List;
  }()) || _class);
});
define('pages/teams/create',['exports', 'aurelia-framework', '../../services/api', 'bootstrap'], function (exports, _aureliaFramework, _api, _bootstrap) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.Create = undefined;

  var _bootstrap2 = _interopRequireDefault(_bootstrap);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var _createClass = function () {
    function defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor) descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }

    return function (Constructor, protoProps, staticProps) {
      if (protoProps) defineProperties(Constructor.prototype, protoProps);
      if (staticProps) defineProperties(Constructor, staticProps);
      return Constructor;
    };
  }();

  var _dec, _class;

  var Create = exports.Create = (_dec = (0, _aureliaFramework.inject)(_api.Api), _dec(_class = function () {
    function Create(api) {
      _classCallCheck(this, Create);

      this.name = 'Test Name';
      this.email = 'test@yahoo.com';
      this.status = '';
      this.divisions = [];

      this._api = api;
    }

    Create.prototype.attached = function attached() {
      var element = (0, _bootstrap2.default)('#myAffix');

      element.affix();

      element.width(element.parent().width());
    };

    Create.prototype.activate = function activate(params) {
      var _this = this;

      return this._api.getDivisions().then(function (result) {
        _this.divisions = result.divisions;
      });
    };

    Create.prototype.createTeam = function createTeam() {
      var _this2 = this;

      this.status = 'Saving...';

      var playerIds = [];

      for (var _iterator = this.divisions, _isArray = Array.isArray(_iterator), _i = 0, _iterator = _isArray ? _iterator : _iterator[Symbol.iterator]();;) {
        var _ref;

        if (_isArray) {
          if (_i >= _iterator.length) break;
          _ref = _iterator[_i++];
        } else {
          _i = _iterator.next();
          if (_i.done) break;
          _ref = _i.value;
        }

        var division = _ref;

        for (var _iterator2 = division.players, _isArray2 = Array.isArray(_iterator2), _i2 = 0, _iterator2 = _isArray2 ? _iterator2 : _iterator2[Symbol.iterator]();;) {
          var _ref2;

          if (_isArray2) {
            if (_i2 >= _iterator2.length) break;
            _ref2 = _iterator2[_i2++];
          } else {
            _i2 = _iterator2.next();
            if (_i2.done) break;
            _ref2 = _i2.value;
          }

          var player = _ref2;

          if (player.selected) playerIds.push(player.id);
        }
      }

      this._api.createTeam({ name: this.name, email: this.email, playerIds: playerIds }).then(function (result) {
        console.info(result);
        _this2.status = 'Done!';
      });
    };

    Create.prototype.loadPlayerStats = function loadPlayerStats(player) {
      if (player.playerTotals === undefined) {
        return this._api.getPlayer(player.id).then(function (result) {
          player.playerTotals = result.player.playerTotals;
        });
      } else return true;
    };

    Create.prototype.scrollToAnchor = function scrollToAnchor(anchorName) {
      var aTag = (0, _bootstrap2.default)("a[name='" + anchorName + "']");
      (0, _bootstrap2.default)('html,body').animate({ scrollTop: aTag.offset().top - 55 }, 'slow');
    };

    Create.prototype.togglePlayer = function togglePlayer(division, player) {
      if (division.selectedCount == null) division.selectedCount = 0;

      if (player.selected) {
        player.selected = false;
        division.selectedCount--;
      } else {
        player.selected = true;
        division.selectedCount++;
      }
    };

    _createClass(Create, [{
      key: 'validLineup',
      get: function get() {
        for (var _iterator3 = this.divisions, _isArray3 = Array.isArray(_iterator3), _i3 = 0, _iterator3 = _isArray3 ? _iterator3 : _iterator3[Symbol.iterator]();;) {
          var _ref3;

          if (_isArray3) {
            if (_i3 >= _iterator3.length) break;
            _ref3 = _iterator3[_i3++];
          } else {
            _i3 = _iterator3.next();
            if (_i3.done) break;
            _ref3 = _i3.value;
          }

          var division = _ref3;

          if (division.selectedCount != division.playerRequirement) return false;
        }

        return true;
      }
    }]);

    return Create;
  }()) || _class);
});
define('pages/welcome/index',["exports"], function (exports) {
  "use strict";

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var Welcome = exports.Welcome = function Welcome() {
    _classCallCheck(this, Welcome);
  };
});
define('resources/elements/baseball-card',['exports', 'aurelia-framework', 'bootstrap', 'flip'], function (exports, _aureliaFramework, _bootstrap) {
    'use strict';

    Object.defineProperty(exports, "__esModule", {
        value: true
    });
    exports.BaseballCard = undefined;

    var _bootstrap2 = _interopRequireDefault(_bootstrap);

    function _interopRequireDefault(obj) {
        return obj && obj.__esModule ? obj : {
            default: obj
        };
    }

    function _initDefineProp(target, property, descriptor, context) {
        if (!descriptor) return;
        Object.defineProperty(target, property, {
            enumerable: descriptor.enumerable,
            configurable: descriptor.configurable,
            writable: descriptor.writable,
            value: descriptor.initializer ? descriptor.initializer.call(context) : void 0
        });
    }

    function _classCallCheck(instance, Constructor) {
        if (!(instance instanceof Constructor)) {
            throw new TypeError("Cannot call a class as a function");
        }
    }

    function _applyDecoratedDescriptor(target, property, decorators, descriptor, context) {
        var desc = {};
        Object['ke' + 'ys'](descriptor).forEach(function (key) {
            desc[key] = descriptor[key];
        });
        desc.enumerable = !!desc.enumerable;
        desc.configurable = !!desc.configurable;

        if ('value' in desc || desc.initializer) {
            desc.writable = true;
        }

        desc = decorators.slice().reverse().reduce(function (desc, decorator) {
            return decorator(target, property, desc) || desc;
        }, desc);

        if (context && desc.initializer !== void 0) {
            desc.value = desc.initializer ? desc.initializer.call(context) : void 0;
            desc.initializer = undefined;
        }

        if (desc.initializer === void 0) {
            Object['define' + 'Property'](target, property, desc);
            desc = null;
        }

        return desc;
    }

    function _initializerWarningHelper(descriptor, context) {
        throw new Error('Decorating class property failed. Please ensure that transform-class-properties is enabled.');
    }

    var _desc, _value, _class, _descriptor, _descriptor2;

    var BaseballCard = exports.BaseballCard = (_class = function () {
        function BaseballCard() {
            _classCallCheck(this, BaseballCard);

            _initDefineProp(this, 'id', _descriptor, this);

            _initDefineProp(this, 'player', _descriptor2, this);
        }

        BaseballCard.prototype.attached = function attached() {
            (0, _bootstrap2.default)('#' + this.id).flip({ trigger: 'click' });
        };

        return BaseballCard;
    }(), (_descriptor = _applyDecoratedDescriptor(_class.prototype, 'id', [_aureliaFramework.bindable], {
        enumerable: true,
        initializer: function initializer() {
            return '';
        }
    }), _descriptor2 = _applyDecoratedDescriptor(_class.prototype, 'player', [_aureliaFramework.bindable], {
        enumerable: true,
        initializer: function initializer() {
            return {};
        }
    })), _class);
});
define('resources/elements/loading',['exports', 'nprogress', 'aurelia-framework'], function (exports, _nprogress, _aureliaFramework) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.Loading = undefined;

  var nprogress = _interopRequireWildcard(_nprogress);

  function _interopRequireWildcard(obj) {
    if (obj && obj.__esModule) {
      return obj;
    } else {
      var newObj = {};

      if (obj != null) {
        for (var key in obj) {
          if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key];
        }
      }

      newObj.default = obj;
      return newObj;
    }
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var Loading = exports.Loading = (0, _aureliaFramework.decorators)((0, _aureliaFramework.noView)(['nprogress/nprogress.css']), (0, _aureliaFramework.bindable)({ name: 'loading', defaultValue: false })).on(function () {
    function _class() {
      _classCallCheck(this, _class);
    }

    _class.prototype.loadingChanged = function loadingChanged(newValue) {
      if (newValue) {
        nprogress.start();
      } else {
        nprogress.done();
      }
    };

    return _class;
  }());
});
define('resources/elements/nav-bar',['exports', 'aurelia-framework', 'moment'], function (exports, _aureliaFramework, _moment) {
	'use strict';

	Object.defineProperty(exports, "__esModule", {
		value: true
	});
	exports.NavBar = undefined;

	var _moment2 = _interopRequireDefault(_moment);

	function _interopRequireDefault(obj) {
		return obj && obj.__esModule ? obj : {
			default: obj
		};
	}

	function _initDefineProp(target, property, descriptor, context) {
		if (!descriptor) return;
		Object.defineProperty(target, property, {
			enumerable: descriptor.enumerable,
			configurable: descriptor.configurable,
			writable: descriptor.writable,
			value: descriptor.initializer ? descriptor.initializer.call(context) : void 0
		});
	}

	function _classCallCheck(instance, Constructor) {
		if (!(instance instanceof Constructor)) {
			throw new TypeError("Cannot call a class as a function");
		}
	}

	function _applyDecoratedDescriptor(target, property, decorators, descriptor, context) {
		var desc = {};
		Object['ke' + 'ys'](descriptor).forEach(function (key) {
			desc[key] = descriptor[key];
		});
		desc.enumerable = !!desc.enumerable;
		desc.configurable = !!desc.configurable;

		if ('value' in desc || desc.initializer) {
			desc.writable = true;
		}

		desc = decorators.slice().reverse().reduce(function (desc, decorator) {
			return decorator(target, property, desc) || desc;
		}, desc);

		if (context && desc.initializer !== void 0) {
			desc.value = desc.initializer ? desc.initializer.call(context) : void 0;
			desc.initializer = undefined;
		}

		if (desc.initializer === void 0) {
			Object['define' + 'Property'](target, property, desc);
			desc = null;
		}

		return desc;
	}

	function _initializerWarningHelper(descriptor, context) {
		throw new Error('Decorating class property failed. Please ensure that transform-class-properties is enabled.');
	}

	var _dec, _class, _desc, _value, _class2, _descriptor, _descriptor2;

	var NavBar = exports.NavBar = (_dec = (0, _aureliaFramework.inject)(), _dec(_class = (_class2 = function () {
		function NavBar() {
			_classCallCheck(this, NavBar);

			_initDefineProp(this, 'router', _descriptor, this);

			_initDefineProp(this, 'lastTeamUpdate', _descriptor2, this);

			this.ticker = null;
			this.updateStatus = '';
		}

		NavBar.prototype.attached = function attached() {
			var _this = this;

			this.setUpdateStatus();
			this.ticker = setInterval(function () {
				return _this.setUpdateStatus();
			}, 60000);
		};

		NavBar.prototype.detached = function detached() {
			if (this.ticker !== null) clearInterval(this.ticker);
		};

		NavBar.prototype.setUpdateStatus = function setUpdateStatus() {
			if (this.lastTeamUpdate === null) this.updateStatus = '';

			if (this.lastTeamUpdate === undefined) this.updateStatus = 'Stat update running now';

			var update = (0, _moment2.default)(this.lastTeamUpdate).toDate();

			this.updateStatus = 'Stats updated ' + (0, _moment2.default)(_moment2.default.utc([update.getFullYear(), update.getMonth(), update.getDate(), update.getHours(), update.getMinutes(), update.getSeconds()]).toDate()).fromNow();
		};

		return NavBar;
	}(), (_descriptor = _applyDecoratedDescriptor(_class2.prototype, 'router', [_aureliaFramework.bindable], {
		enumerable: true,
		initializer: function initializer() {
			return null;
		}
	}), _descriptor2 = _applyDecoratedDescriptor(_class2.prototype, 'lastTeamUpdate', [_aureliaFramework.bindable], {
		enumerable: true,
		initializer: function initializer() {
			return null;
		}
	})), _class2)) || _class);
});
define('resources/value-converters/age',['exports', 'moment'], function (exports, _moment) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.AgeValueConverter = undefined;

  var _moment2 = _interopRequireDefault(_moment);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var AgeValueConverter = exports.AgeValueConverter = function () {
    function AgeValueConverter() {
      _classCallCheck(this, AgeValueConverter);
    }

    AgeValueConverter.prototype.toView = function toView(value) {
      return (0, _moment2.default)().diff((0, _moment2.default)(value), 'years');
    };

    return AgeValueConverter;
  }();
});
define('resources/value-converters/baseball-card-id',["exports"], function (exports) {
  "use strict";

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var BaseballCardIdValueConverter = exports.BaseballCardIdValueConverter = function () {
    function BaseballCardIdValueConverter() {
      _classCallCheck(this, BaseballCardIdValueConverter);
    }

    BaseballCardIdValueConverter.prototype.toView = function toView(value) {
      return value.toString().substring(0, 3);
    };

    return BaseballCardIdValueConverter;
  }();
});
define('resources/value-converters/date-format',['exports', 'moment'], function (exports, _moment) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.DateFormatValueConverter = undefined;

  var _moment2 = _interopRequireDefault(_moment);

  function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : {
      default: obj
    };
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var DateFormatValueConverter = exports.DateFormatValueConverter = function () {
    function DateFormatValueConverter() {
      _classCallCheck(this, DateFormatValueConverter);
    }

    DateFormatValueConverter.prototype.toView = function toView(value, format) {
      return (0, _moment2.default)(value).format(format);
    };

    return DateFormatValueConverter;
  }();
});
define('resources/value-converters/home-away',['exports'], function (exports) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var HomeAwayValueConverter = exports.HomeAwayValueConverter = function () {
    function HomeAwayValueConverter() {
      _classCallCheck(this, HomeAwayValueConverter);
    }

    HomeAwayValueConverter.prototype.toView = function toView(value) {
      return value.toUpperCase() === 'H' ? 'vs' : 'at';
    };

    return HomeAwayValueConverter;
  }();
});
define('resources/value-converters/roundAvg',["exports"], function (exports) {
  "use strict";

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var RoundAvgValueConverter = exports.RoundAvgValueConverter = function () {
    function RoundAvgValueConverter() {
      _classCallCheck(this, RoundAvgValueConverter);
    }

    RoundAvgValueConverter.prototype.toView = function toView(value, places) {
      return value.toFixed(places).toString().substring(1);
    };

    return RoundAvgValueConverter;
  }();
});
define('resources/value-converters/win-loss',['exports'], function (exports) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var WinLossValueConverter = exports.WinLossValueConverter = function () {
    function WinLossValueConverter() {
      _classCallCheck(this, WinLossValueConverter);
    }

    WinLossValueConverter.prototype.toView = function toView(value) {
      return value.toUpperCase() === 'W' ? 'win' : 'loss';
    };

    return WinLossValueConverter;
  }();
});
define('text!styles.css', ['module'], function(module) { module.exports = "/* Fonts */\n@font-face {\n  font-family: \"mlb_primary\";\n  font-style: normal;\n  font-weight: bold;\n  src: url(\"../assets/fonts/6ec61f10-00ea-4ffd-a36b-26e2577a83f1-2.eot\");\n  src: url(\"../assets/fonts/6ec61f10-00ea-4ffd-a36b-26e2577a83f1-2.eot?#iefix\") format('embedded-opentype'),\n  url(\"../assets/fonts/6ec61f10-00ea-4ffd-a36b-26e2577a83f1-3.woff\") format('woff'),\n  url(\"../assets/fonts/6ec61f10-00ea-4ffd-a36b-26e2577a83f1-1.ttf\") format('truetype'),\n  url(\"../assets/fonts/6ec61f10-00ea-4ffd-a36b-26e2577a83f1-4.svg#web\") format('svg');\n}\n\nbody { \n  font-family: mlb_primary;\n  padding-top: 60px;\n  background: #fdfdff url(\"../assets/images/mlb_bg_gradient.png\") fixed repeat-x; \n}\n\n.navbar-main {\n  box-shadow: 0 0 5px rgba(0, 0, 0, .5);\n  background-color: #F8F8F8 !important; \n}\n\n.pad-top{ \n  padding-top: 25px;\n}\n\n.stat {\n  font-family: helvetica,arial,sans-serif;\n}\n\n.container {\n  background-color: white;\n}\n\n.content {\n  padding: 10px;\n}\n\n.splash {\n  text-align: center;\n  margin: 10% 0 0 0;\n  box-sizing: border-box;\n}\n\n.splash .message {\n  font-size: 72px;\n  line-height: 72px;\n  text-shadow: rgba(0, 0, 0, 0.5) 0 0 15px;\n  text-transform: uppercase;\n  font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif;\n}\n\n.splash .fa-spinner {\n  text-align: center;\n  display: inline-block;\n  font-size: 72px;\n  margin-top: 50px;\n}\n\n.page-host {\n  position: absolute;\n  left: 0;\n  right: 0;\n  top: 50px;\n  bottom: 0;\n  overflow-x: hidden;\n  overflow-y: auto;\n}\n\n@media print {\n  .page-host {\n    position: absolute;\n    left: 10px;\n    right: 0;\n    top: 50px;\n    bottom: 0;\n    overflow-y: inherit;\n    overflow-x: inherit;\n  }\n}\n\nsection {\n  margin: 0 20px;\n}\n\n.navbar-nav li.loader {\n  margin: 12px 24px 0 6px;\n}\n\n/* animate page transitions */\nsection.au-enter-active {\n  -webkit-animation: fadeInRight 1s;\n  animation: fadeInRight 1s;\n}\n\ndiv.au-stagger {\n  /* 50ms will be applied between each successive enter operation */\n  -webkit-animation-delay: 50ms;\n  animation-delay: 50ms;\n}\n\n.card-container.au-enter {\n  opacity: 0;\n}\n\n.card-container.au-enter-active {\n  -webkit-animation: fadeIn 2s;\n  animation: fadeIn 2s;\n}\n\n.card {\n  overflow: hidden;\n  position: relative;\n  border: 1px solid #CCC;\n  border-radius: 8px;\n  text-align: center;\n  padding: 0;\n  background-color: #337ab7;\n  color: rgb(136, 172, 217);\n  margin-bottom: 32px;\n  box-shadow: 0 0 5px rgba(0, 0, 0, .5);\n}\n\n.card .content {\n  margin-top: 10px;\n}\n\n.card .content .name {\n  color: white;\n  text-shadow: 0 0 6px rgba(0, 0, 0, .5);\n  font-size: 18px;\n}\n\n.card .header-bg {\n  /* This stretches the canvas across the entire hero unit */\n  position: absolute;\n  top: 0;\n  left: 0;\n  width: 100%;\n  height: 70px;\n  border-bottom: 1px #FFF solid;\n  border-radius: 6px 6px 0 0;\n}\n\n.card .avatar {\n  position: relative;\n  margin-top: 15px;\n  z-index: 100;\n}\n\n.card .avatar img {\n  width: 100px;\n  height: 100px;\n  -webkit-border-radius: 50%;\n  -moz-border-radius: 50%;\n  border-radius: 50%;\n  border: 2px #FFF solid;\n}\n\n/* animation definitions */\n@-webkit-keyframes fadeInRight {\n  0% {\n    opacity: 0;\n    -webkit-transform: translate3d(100%, 0, 0);\n    transform: translate3d(100%, 0, 0)\n  }\n  100% {\n    opacity: 1;\n    -webkit-transform: none;\n    transform: none\n  }\n}\n\n@keyframes fadeInRight {\n  0% {\n    opacity: 0;\n    -webkit-transform: translate3d(100%, 0, 0);\n    -ms-transform: translate3d(100%, 0, 0);\n    transform: translate3d(100%, 0, 0)\n  }\n  100% {\n    opacity: 1;\n    -webkit-transform: none;\n    -ms-transform: none;\n    transform: none\n  }\n}\n\n@-webkit-keyframes fadeIn {\n  0% {\n    opacity: 0;\n  }\n  100% {\n    opacity: 1;\n  }\n}\n\n@keyframes fadeIn {\n  0% {\n    opacity: 0;\n  }\n  100% {\n    opacity: 1;\n  }\n}\n\n/* SPINNER */\n.glyphicon.spinning {\n    animation: spin 1s infinite linear;\n    -webkit-animation: spin2 1s infinite linear;\n}\n\n@keyframes spin {\n    from { transform: scale(1) rotate(0deg); }\n    to { transform: scale(1) rotate(360deg); }\n}\n\n@-webkit-keyframes spin2 {\n    from { -webkit-transform: rotate(0deg); }\n    to { -webkit-transform: rotate(360deg); }\n}\n\n/* CARDS */\ndiv.card-container {\n  margin: 3rem;\n\tposition: relative;\n\twidth:248px;\n\theight:341px;\n  cursor: pointer;\n}\n\n.selected{\n   box-shadow:5px 12px 22px 1px #002e6d !important;\n}\n\ndiv.card-frame {\n\tposition: absolute;\n\tleft: 0;\n\ttop: 0;\n\twidth:248px;\n\theight:341px;\t\n\tbackground: url(\"../assets/images/cardframe.png\");\n\tz-index: 1000;\n  \n  box-shadow:0px 8px 22px 1px #333;\n}\n\ndiv.card-back {\n\tleft: 0;\n\ttop: 0;\n\twidth:248px;\n\theight:341px;\n  background: url(\"http://i.imgur.com/awmsxbC.png\");\t\n  box-shadow:0px 8px 22px 1px #333;\n}\n\ndiv.card-back > div.card-back-nameplate {\n  position: absolute;\n  left: 62px;\n  top: 7px;\n  font-size: 17px;\n  color: #808D48;\n}\n\ndiv.card-back > div.card-back-id {\n  position: absolute;\n  left: 25px;\n  top: 8px;\n  width: 30px;\n  font-size: 16px;\n  color: #1f3542;\n  text-align: center;\n}\ndiv.card-back a {\n  color: #1f3542;\n}\n\ndiv.card-back-body\n{\n  position: absolute;\n  left: 25px;\n  top: 35px;\n  width: 205px;\n  font-size: 12px;\n  color: #1f3542;\n}\n\ndiv.card-back-body > table\n{\n  border-collapse:separate; \n  border-spacing:0;\n  font-family: Arial, Helvetica, sans-serif;\n  font-size: .8em;\n  font-weight: bold;\n  text-align: right;\n  text-transform: uppercase;\n  width: 100%;\n}\n\ncaption {\n  color: #1f3542;\n  font-size: 1.1em;\n  padding: 0;\n  text-align: center;\n}\n\ndiv.card-back-body > table th\n{ \n  border-bottom: 1px solid;\n  text-align: right;\n}\n\ndiv.card-back-body > table td\n{\n  \n}\n\nimg.card-logo {\n\ttop: 20px;\n\tleft: 20px;\n  position: absolute;\n\twidth:50px;\n\theight:50px;\t\n\tz-index: 1000;\n}\n\nimg.card-picture {\n\ttop: 15px;\n\tleft: 21px;\n\tposition: absolute;\n\twidth:213px;\n\theight:320px;\t\n}\n\ndiv.card-nameplate {\n  display: flex;\n  justify-content:center;\n  align-items:center;\n\ttop: 295px;\n\tleft: 73px;\n\tposition: absolute;\n\tbackground-color:blue;\n\twidth:157px;\n\theight:34px;\n}\n\ndiv.card-nameplate span.playername {\n  text-transform: uppercase;\n  color: white;\n  font-weight: bold;\n  font-size: 0.9em;\n  padding: 0 3 0 3;\n}\n\nimg.img-player {\n  box-shadow: 3px 3px 1px #888888;\n}\n\n.list-row {\n  margin-top:20px;\n}\n\n.list-lead {\n  font-size: 36px;\n  font-weight: 500;\n  line-height: 1.2;\n  margin-top: 20px;\n  margin-bottom: 10px;\n}\n\np.list-lead-note {\n  margin: -12px 0 0 0;\n}\n\n.hr-count {\n  font-family: helvetica,arial,sans-serif;\n  font-size: 36px; \n  border-bottom: 5px solid #c00; \n  margin-top: 15px; \n  display: inline-block;\n}\n\n.rank {\n  margin-left: 20px;\n  text-align: center;\n  min-width: 50px;\n  font-size: 36px; \n  color: azure;\n  background-color: #002e6d; \n  margin-top: 20px; \n  display: inline-block;\n}\n\n.profile-bio-container {\n  display: flex;\n  align-items: center;\n}\n\n.profile-stat-line {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n}\n\n.profile-stat {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  height: 70px;\n  width: 70px;\n  font-size: 32px;\n  margin: 10px 10px;\n  \n  box-shadow: 3px 3px 1px #888888;\n}\n\n.profile-stat-label {\n  color: azure;\n  background-color: #002e6d;\n}\n\n.profile-stat-stat {\n  color: #002e6d;\n  background-color: azure;\n  border: 2px solid #002e6d;\n}\n\n.profile-stat-spacer {\n  width: 20px;\n}\n\n.pointy {\n  cursor: pointer;\n}\n\n/* HEADINGS */\n/* h1 {\n  color: #002e6d;\n  border-bottom: double #555;\n} */\n\nh2.division {\n  color: #002e6d;\n  border-bottom: double #555; \n}\n\nh2.division > span.requirement {\n  margin-top: 16px;\n  font-size: .5em;\n}\n\nh2.division > span.valid{\n  padding-right: 5px;\n}"; });
define('text!app.html', ['module'], function(module) { module.exports = "<template>\n\t<require from=\"bootstrap/css/bootstrap.css\"></require>\n\t<require from=\"./styles.css\"></require>\n\t<nav-bar router.bind=\"router\" last-team-update.bind=\"lastTeamUpdate\"></nav-bar>\n\t<loading loading.bind=\"router.isNavigating || api.isRequesting\"></loading>\n\t<div class=\"container\">\n\t\t<div class=\"content\">\n\t\t\t<router-view>\n\t\t\t</router-view>\n\t\t</div>\n\t</div>\n</template>"; });
define('text!pages/activity/activity.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"../../resources/value-converters/date-format\"></require>\n  <require from=\"../../resources/value-converters/home-away\"></require>\n  <require from=\"../../resources/value-converters/win-loss\"></require>\n  <div if.bind=\"recent.length === 0\">\n    <h1>There have not been any home runs hit in the past week.</h1>\n  </div>\n  \n  <div repeat.for=\"stat of recent\">\n    <div class=\"list-row\">\n      <div if.bind=\"recent[$index-1].gameDate != stat.gameDate\" class=\"row\">\n        <div class=\"col-md-6 col-md-offset-3\">\n          <h2>\n            ${stat.gameDate | dateFormat:'dddd, MMMM Do'}\n          </h2>\n          <hr/>\n        </div>\n      </div>\n      <div class=\"row\">\n        <div class=\"col-md-1 col-md-offset-3\">\n          <div class=\"hr-count pull-right\">\n            ${stat.hr}\n          </div>\n        </div>\n        <div class=\"col-md-1\"> \n          <img src=\"${stat.playerImage}\" class=\"img-player img-responsive\" />\n        </div>\n        <div class=\"col-md-6\">\n          <div class=\"list-lead\">${stat.displayName}</div>\n          <p>In a ${stat.teamScore} - ${stat.oppnentScore} ${stat.result | winLoss} ${stat.homeAway | homeAway} the ${stat.opponent}</p>\n        </div>\n      </div>\n    </div>\n</template>"; });
define('text!pages/leaders/homeruns.html', ['module'], function(module) { module.exports = "<template>\n  <div repeat.for=\"player of leaders\" class=\"row\">\n    <div class=\"list-row\">\n      <div class=\"col-md-1 col-md-offset-3\">\n        <div class=\"hr-count pull-right\">\n          ${player.hr}\n        </div>\n      </div>\n      <div class=\"col-md-1\">\n        <img src=\"${player.playerImage}\" class=\"img-player img-responsive\" />\n      </div>\n      <div class=\"col-md-6\">\n        <button click.delegate=\"toggleHistory(player)\" class=\"btn btn-link\" style=\"outline: none;\">\n          <div class=\"list-lead\">${player.displayName}</div> \n        </button>\n        <a if.bind=\"player.hr7 >= 3\" class='my-tool-tip' data-toggle=\"tooltip\" data-placement=\"top\" title=\"3+ HRs in past 7 days.\">\n          <span class=\"glyphicon glyphicon-fire\" style=\"color:#E25822\"></span>\n        </a>\n        <a if.bind=\"player.hr14 == 0\" class='my-tool-tip' data-toggle=\"tooltip\" data-placement=\"top\" title=\"No HRs in past 14 days.\">\n          <span class=\"glyphicon glyphicon-asterisk\" style=\"color:#AAD4E5\"></span>\n        </a>\n        <span id=\"spinner-${player.playerId}\" style=\"display:none;\" class=\"glyphicon glyphicon-cog spinning\"></span>\n        <div id=\"history-${player.playerId}\" style=\"display:none;\" class=\"col-md-9\">\n          <hr/>\n          <div class=\"pull-left\">\n            <div repeat.for=\"item of player.history.recentHr\">\n              <div class=\"stat\">\n                <u>${item.date}</u> - <b>${item.hr}</b> ${item.location} ${item.opp}\n              </div>\n            </div>\n          </div>\n          <div class=\"pull-right stat\">\n            <div class=\"text-right\">\n              Last\n              <u>7</u> Days: <b>${player.hr7}</b>\n            </div>\n            <div class=\"text-right\">\n              Last\n              <u>14</u> Days: <b>${player.hr14}</b>\n            </div>\n            <div class=\"text-right\">\n              Last\n              <u>30</u> Days: <b>${player.hr30}</b>\n            </div>\n            <div class=\"text-right\">\n              <br/>\n              <a route-href=\"route: player; params.bind: {id:player.playerId}\">Player Profile</a>\n            </div>\n\n          </div>\n          <div class=\"clearfix\"></div>\n          <hr/>\n        </div>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/players/details.html', ['module'], function(module) { module.exports = "<template>\n\t<require from=\"../../resources/value-converters/roundAvg\"></require>\n\t<require from=\"../../resources/value-converters/age\"></require>\n\t<require from=\"../../resources/value-converters/date-format\"></require>\n\t<div if.bind=\"!player\">\n\t\t<div class=\"row\">\n\t\t\t<div class=\"col-md-12\">\n\t\t\t\t<h1>Player Not Found</h1>\n\t\t\t</div>\n\t\t</div>\n\t</div>\n\t<div if.bind=\"player\">\n\t\t<div class=\"row\">\n\t\t\t<div class=\"col-md-2\">\n\t\t\t\t<img src=\"${player.playerImage}\" class=\"img-player img-responsive\" />\n\t\t\t</div>\n\t\t\t<div class=\"col-md-4\">\n\t\t\t\t<div class=\"profile-bio-container\">\n\t\t\t\t\t<div>\n\t\t\t\t\t\t<h1>${player.displayName} | #${player.jerseyNumber}</h1>\n\t\t\t\t\t\t<h2>${player.teamName}</h2>\n\t\t\t\t\t\t<p>\n\t\t\t\t\t\t\t<strong>Height</strong>: ${player.heightFeet}' ${player.heightFeet}\"&nbsp;\n\t\t\t\t\t\t\t<strong>Weight</strong>: ${player.weight}&nbsp;\n\t\t\t\t\t\t\t<strong>Age</strong>: ${player.birthDate | age}&nbsp;\n\t\t\t\t\t\t</p>\n\t\t\t\t\t\t<p>\n\t\t\t\t\t\t\t<strong>Born</strong>: ${player.birthDate | dateFormat:'M/D/YYYY'}\n\t\t\t\t\t\t</p>\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t\t<div class=\"col-md-5\">\n\n\t\t\t\t<div if.bind=\"player.playerTotals.length > 0\">\n\t\t\t\t\t<div class=\"profile-stat-line\">\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">G</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].g}</div>\n\t\t\t\t\t\t<div class=\"profile-stat-spacer\"></div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">AB</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].ab}</div>\n\t\t\t\t\t</div>\n\t\t\t\t\t<div class=\"profile-stat-line\">\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">H</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].h}</div>\n\t\t\t\t\t\t<div class=\"profile-stat-spacer\"></div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">HR</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].hr}</div>\n\t\t\t\t\t</div>\n\t\t\t\t\t<div class=\"profile-stat-line\">\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">AVG</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].avg | roundAvg:3}</div>\n\t\t\t\t\t\t<div class=\"profile-stat-spacer\"></div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-label\">SLG</div>\n\t\t\t\t\t\t<div class=\"profile-stat profile-stat-stat\">${player.playerTotals[0].slg | roundAvg:3}</div>\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\n\t\t\t</div>\n\t\t</div>\n\t\t<div class=\"row\">\n\t\t\t<div class=\"col-md-12\">\n\t\t\t\t<hr/>\n\t\t\t\t<h2>Yearly Statistics</h2>\n\t\t\t\t<div class=\"table-responsive\">\n\t\t\t\t\t<table class=\"table table-striped table-hover\">\n\t\t\t\t\t\t<thead>\n\t\t\t\t\t\t\t<tr>\n\t\t\t\t\t\t\t\t<th>YEAR</th>\n\t\t\t\t\t\t\t\t<th>G</th>\n\t\t\t\t\t\t\t\t<th>AB</th>\n\t\t\t\t\t\t\t\t<th>R</th>\n\t\t\t\t\t\t\t\t<th>H</th>\n\t\t\t\t\t\t\t\t<th>TB</th>\n\t\t\t\t\t\t\t\t<th>HR</th>\n\t\t\t\t\t\t\t\t<th>RBI</th>\n\t\t\t\t\t\t\t\t<th>BB</th>\n\t\t\t\t\t\t\t\t<th>IBB</th>\n\t\t\t\t\t\t\t\t<th>SO</th>\n\t\t\t\t\t\t\t\t<th>SB</th>\n\t\t\t\t\t\t\t\t<th>CS</th>\n\t\t\t\t\t\t\t\t<th>AVG</th>\n\t\t\t\t\t\t\t\t<th>SLG</th>\n\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t</thead>\n\t\t\t\t\t\t<tbody>\n\t\t\t\t\t\t\t<tr repeat.for=\"stat of player.playerTotals\" click.delegate=\"bindCharts(stat)\" class=\"pointy\" class.bind=\"stat.isSelected ? 'warning' : ''\">\n\t\t\t\t\t\t\t\t<td>${stat.year}</td>\n\t\t\t\t\t\t\t\t<td>${stat.g}</td>\n\t\t\t\t\t\t\t\t<td>${stat.ab}</td>\n\t\t\t\t\t\t\t\t<td>${stat.r}</td>\n\t\t\t\t\t\t\t\t<td>${stat.h}</td>\n\t\t\t\t\t\t\t\t<td>${stat.tb}</td>\n\t\t\t\t\t\t\t\t<td>${stat.hr}</td>\n\t\t\t\t\t\t\t\t<td>${stat.rbi}</td>\n\t\t\t\t\t\t\t\t<td>${stat.bb}</td>\n\t\t\t\t\t\t\t\t<td>${stat.ibb}</td>\n\t\t\t\t\t\t\t\t<td>${stat.so}</td>\n\t\t\t\t\t\t\t\t<td>${stat.sb}</td>\n\t\t\t\t\t\t\t\t<td>${stat.cs}</td>\n\t\t\t\t\t\t\t\t<td>${stat.avg | roundAvg:3}</td>\n\t\t\t\t\t\t\t\t<td>${stat.slg | roundAvg:3}</td>\n\t\t\t\t\t\t\t</tr>\n\t\t\t\t\t\t</tbody>\n\t\t\t\t\t</table>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</div>\n\n\t\t<div class=\"row\" if.bind=\"player.playerTotals.length > 0\">\n\t\t\t<div class=\"col-md-6\">\n\t\t\t\t<fieldset>\n\t\t\t\t\t<legend>${statsYear} Home Runs</legend>\n\t\t\t\t\t<chart id=\"dynamic-radar-chart-1\" type=\"radar\" style=\"width: 100%; height: auto; display: block;\" should-update=\"true\" throttle=\"100\"\n\t\t\t\t\t\tdata.bind=\"radarHrData\" native-options.bind=\"{ legend: { display: false } }\"></chart>\n\t\t\t\t</fieldset>\n\t\t\t</div>\n\t\t\t<div class=\"col-md-6\">\n\t\t\t\t<fieldset>\n\t\t\t\t\t<legend>${statsYear} At Bats</legend>\n\t\t\t\t\t<chart id=\"dynamic-radar-chart-2\" type=\"radar\" style=\"width: 100%; height: auto; display: block;\" should-update=\"true\" throttle=\"100\"\n\t\t\t\t\t\tdata.bind=\"radarAbData\" native-options.bind=\"{ legend: { display: false } }\"></chart>\n\t\t\t\t</fieldset>\n\t\t\t</div>\n\t\t</div>\n\t</div>\n</template>"; });
define('text!pages/standings/list.html', ['module'], function(module) { module.exports = "<template>\n  <div repeat.for=\"team of teams\" class=\"row\">\n    <div class=\"list-row\">\n      <div class=\"col-md-1 col-md-offset-3\">\n        <div class=\"rank\">\n          ${$index + 1}\n        </div>\n      </div>\n      <div class=\"col-md-1\">\n        <div class=\"hr-count\">\n          ${team.totals.hr}<br />\n        </div>\n        <sup if.bind=\"team.totals.hrMovement > 0\" class=\"text-success\" style=\"position: absolute; font-size: 1.1em; top: 25px; left: 75px;\">+${team.totals.hrMovement}</sup>\n      </div>\n      <div class=\"col-md-7\">\n        <div class=\"list-lead\">${team.name}</div>\n        <p if.bind=\"teams[$index + 1].totals.hr == team.totals.hr || teams[$index - 1].totals.hr == team.totals.hr\" class=\"list-lead-note\">\n          <span class=\"glyphicon glyphicon-option-horizontal\"></span>&nbsp;\n          <span class=\"text-info\">Total AB: ${team.totals.ab.toLocaleString()}</span>\n        </p>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/teams/create.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"../../resources/elements/baseball-card\"></require>\n  <div class=\"row\">\n    <div class=\"content col-md-12 pad-top\">\n      \n        <p class=\"lead\">\n          So you think you've got what it takes to dominate the Homerun League? It's time to prove it. You will need to build a\n          team first. In order to do that, pick the required number of players from each division. Next, \n          pick a team name (be creative!), fill out your email address, and confirm your lineup. Finally, all that\n          there's left to do is enjoy the baseball season and root for your team in the standings!\n        </p>\n      \n    </div>\n  </div>\n  <div class=\"row\">\n    <div class=\"col-md-10\">\n      <div repeat.for=\"division of divisions\">\n        <a name=\"div-${division.id}\"></a>\n        <h2 class=\"division\">${division.name} Division\n          <span class=\"requirement pull-right\" class.bind=\"division.selectedCount == division.playerRequirement ? 'text-success' : 'text-danger'\">\n            Pick ${division.playerRequirement} players\n           </span>\n          <span class=\"requirement valid text-success pull-right glyphicon glyphicon-ok\" if.bind=\"division.selectedCount == division.playerRequirement\"></span>\n        </h2>\n\n        <div repeat.for=\"player of division.players\">\n          <div class=\"pull-left\">\n            <baseball-card id.one-way=\"'player-card-' + player.id\" player.bind=\"player\" click.delegate=\"loadPlayerStats(player)\"></baseball-card>\n            <button disabled.bind=\"division.selectedCount >= division.playerRequirement && !player.selected\" class.bind=\"player.selected ? 'btn-success glyphicon-ok' : 'glyphicon-minus'\"\n              class=\"btn btn-default btn-lg glyphicon\" click.delegate=\"togglePlayer(division, player)\"></button>\n          </div>\n        </div>\n        <div class=\"clearfix\"></div>\n      </div>\n    </div>\n    <div class=\"col-md-2\">\n      <div id=\"myAffix\">\n\n        <div repeat.for=\"division of divisions\">\n          <div class.bind=\"division.selectedCount == division.playerRequirement ? 'text-success' : ''\">\n            <a href=\"#\" click.delegate=\"scrollToAnchor('div-' + division.id)\">${division.name}</a>: ${division.selectedCount\n            == null ? '0' : division.selectedCount}/${division.playerRequirement}\n            <span class=\"text-success glyphicon glyphicon-ok\" if.bind=\"division.selectedCount == division.playerRequirement\"></span>\n          </div>\n        </div>\n        <hr/>\n        <form>\n          <div class=\"form-group\">\n            <label class=\"sr-only\" for=\"teamName\">Team Name</label>\n            <input type=\"text\" class=\"form-control\" id=\"teamName\" placeholder=\"Team Name\" value.bind=\"name\">\n          </div>\n          <div class=\"form-group\">\n            <label class=\"sr-only\" for=\"email\">Password</label>\n            <input type=\"email\" class=\"form-control\" id=\"email\" placeholder=\"Email Address\" value.bind=\"email\">\n          </div>\n          <div class=\"form-group\">\n            <button disabled.bind=\"!validLineup\" click.trigger=\"createTeam()\" type=\"submit\" class=\"btn btn-primary\">Create Team</button>\n          </div>\n        </form>\n        <hr/>\n        <div>\n          Team creation placeholder. ${validLineup}\n        </div>\n        <div>\n          ${status}\n        </div>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/welcome/index.html', ['module'], function(module) { module.exports = "<template>\n  <div class=\"teamname test\">Paul Konerko 1234567890</div>\n  <div class=\"teamname test2\">Jake Arrieta 1234567890</div>\n  <div class=\"teamname test3\">Ken Griffey Jr. 1234567890</div>\n</template>\n"; });
define('text!resources/elements/baseball-card.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"../../resources/value-converters/baseball-card-id\"></require>\n  <require from=\"../../resources/value-converters/roundAvg\"></require>\n  <div id.bind=\"id\" class=\"card-container\">\n    <div class=\"front\">\n      <div class=\"card-frame\"></div>\n      <img class=\"card-logo\" src=\"${player.teamLogo2X}\" />\n      <img class=\"card-picture\" src=\"${player.playerImage}\" />\n      <div class=\"card-nameplate\">\n        <span class=\"playername\">\n            ${player.displayName}\n          </span>\n      </div>\n    </div>\n    <div class=\"back\">\n      <div class=\"card-back\">\n        <div class=\"card-back-id\">${player.mlbId | baseballCardId}</div>\n        <div class=\"card-back-nameplate\">\n          ${player.displayName}&nbsp;&#9679;&nbsp;${player.primaryPosition}\n        </div>\n        <div class=\"card-back-body\">\n          <table>\n            <caption>Major League Batting Record</caption>\n            <thead>\n              <tr>\n                <th>Year</th>\n                <th>G</th>\n                <th>AB</th>\n                <th>H</th>\n                <th>HR</th>\n                <th>BB</th>\n                <th>SLG</th>\n                <th>SO</th>\n                <th>AVG</th>\n              </tr>\n            </thead>\n            <tr repeat.for=\"stat of player.playerTotals\">\n              <td>${stat.year}</td>\n              <td>${stat.g}</td>\n              <td>${stat.ab}</td>\n              <td>${stat.h}</td>\n              <td>${stat.hr}</td>\n              <td>${stat.bb}</td>\n              <td>${stat.slg | roundAvg:3}</td>\n              <td>${stat.so}</td>\n              <td>${stat.avg | roundAvg:3}</td>\n            </tr>\n          </table>\n          <br/>\n          <div class=\"pull-right\">\n            <a href=\"${player.mlbProfile}\" target=\"_blank\" />MLB Profile</a> | \n            <a route-href=\"route: player; params.bind: {id:player.id}\" target=\"_blank\">Profile</a>\n          </div>\n        </div>\n      </div>\n</template>"; });
define('text!resources/elements/nav-bar.html', ['module'], function(module) { module.exports = "<template>\n\t<header class=\"navbar-fixed-top\">\n\t\t<div class=\"navbar-main container\">\n\t\t\t<nav class=\"navbar-default\">\n\t\t\t\t<div class=\"navbar-header\">\n\t\t\t\t\t<button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#navbar-collapse\" aria-expanded=\"false\">\n\t\t\t\t\t<span class=\"sr-only\">Toggle navigation</span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t</button>\n\t\t\t\t\t<a href=\"#\" class=\"navbar-brand\">\n\t\t\t\t\t\t<img src=\"assets/images/hrd.png\" />\n\t\t\t\t\t</a>\n\t\t\t\t</div>\n\t\t\t\t<div class=\"collapse navbar-collapse\" id=\"navbar-collapse\">\n\t\t\t\t\t<ul class=\"nav navbar-nav\">\n\t\t\t\t\t\t<li repeat.for=\"row of router.navigation\" class=\"${row.isActive ? 'active' : ''}\">\n\t\t\t\t\t\t\t<a href.bind=\"row.href\" data-toggle=\"collapse\" data-target=\".navbar-collapse.in\">${row.title}</a>\n\t\t\t\t\t\t</li>\n\t\t\t\t\t</ul>\n\t\t\t\t\t<p class=\"navbar-text pull-right\"><span class=\"text-info\">${updateStatus}</span></p>\n\t\t\t\t</div>\n\t\t\t</nav>\n\t\t</div>\n\t</header>\n\n</template>"; });
//# sourceMappingURL=app-bundle.js.map