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

      this.api = api;

      eventAggregator.subscribe('router:navigation:complete', function (payload) {
        return window.scrollTo(0, 0);
      });
    }

    App.prototype.configureRouter = function configureRouter(config, router) {
      config.title = 'Homerun League';
      config.map([{ route: ['', 'welcome'], name: 'welcome', moduleId: 'pages/welcome/index', nav: true, title: 'Welcome' }, { route: 'teams/create', name: 'join', moduleId: 'pages/teams/create', nav: true, title: 'Join League' }, { route: 'leaders', name: 'leaders', moduleId: 'pages/leaders/homeruns', nav: true, title: 'Leaders' }, { route: 'standings', name: 'standings', moduleId: 'pages/standings/list', nav: true, title: 'Standings' }, { route: 'activity', name: 'activity', moduleId: 'pages/activity/activity', nav: true, title: 'Activity' }]);

      this.router = router;
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
    api: 'http://192.168.11.134:9001/api/',
    year: 2016
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
    aurelia.use.standardConfiguration().feature('resources');

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

			this.year = environment.year;

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

			return this.http.fetch('seasons/' + this.year + '/recent?page=' + page);
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

          _this.recent = _this.recent.concat(results.RecentHrs);

          if (results.Meta.Page < results.Meta.TotalPages) return loadRecent(results.Meta.Page + 1);
        });
      };

      return loadRecent();
    };

    return Activity;
  }()) || _class);
});
define('pages/leaders/homeruns',['exports', 'aurelia-framework', '../../services/api', 'bootstrap', 'moment'], function (exports, _aureliaFramework, _api, _bootstrap, _moment) {
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

  var HomeRuns = exports.HomeRuns = (_dec = (0, _aureliaFramework.inject)(_api.Api), _dec(_class = function () {
    function HomeRuns(api) {
      _classCallCheck(this, HomeRuns);

      this.api = api;
      this.leaders = [];
    }

    HomeRuns.prototype.activate = function activate(params) {
      var _this = this;

      var loadLeaders = function loadLeaders(page) {
        return _this.api.getLeaders(page).then(function (results) {

          _this.leaders = _this.leaders.concat(results.Leaders);

          if (results.Meta.Page < results.Meta.TotalPages) return loadLeaders(results.Meta.Page + 1);
        });
      };

      return loadLeaders().then(function () {
        return _this.initHistory();
      });
    };

    HomeRuns.prototype.attached = function attached() {
      (0, _bootstrap2.default)('[data-toggle="tooltip"]').tooltip();
    };

    HomeRuns.prototype.initHistory = function initHistory() {
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

    HomeRuns.prototype.loadHistory = function loadHistory(player, gamelogs) {
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

        var gameDate = (0, _moment2.default)(gamelog.GameDate);

        if (player.history.recentHr.length < 5 && gamelog.Hr > 0) {
          var location = gamelog.HomeAway === 'A' ? 'at' : 'vs';
          player.history.recentHr.push({ "date": gameDate.format('MM/DD'), "hr": gamelog.Hr, "location": location, "opp": gamelog.Opponent });
        }
      }

      player.history.loaded = true;
    };

    HomeRuns.prototype.toggleHistory = function toggleHistory(player) {
      var _this2 = this;

      if (!(player.PlayerId in history)) {
        this.api.getGameLogsForPlayer(player.PlayerId).then(function (results) {
          _this2.loadHistory(player, results.GameLogs);
          (0, _bootstrap2.default)('#history-' + player.PlayerId).slideToggle('fast', function () {});
        });
      }
    };

    return HomeRuns;
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

      this.api = api;
      this.teams = [];
    }

    List.prototype.activate = function activate(params) {
      var _this = this;

      var loadData = function loadData(page) {
        return _this.api.getTeams(page).then(function (results) {

          _this.teams = _this.teams.concat(results.Teams);

          if (results.Meta.Page < results.Meta.TotalPages) return loadData(results.Meta.Page + 1);
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

      this.api = api;
      this.name = 'Test Name';
      this.email = 'test@yahoo.com';
      this.status = '';
      this.divisions = [];
    }

    Create.prototype.attached = function attached() {
      var element = (0, _bootstrap2.default)('#myAffix');

      element.affix();

      element.width(element.parent().width());
    };

    Create.prototype.activate = function activate(params) {
      var _this = this;

      return this.api.getDivisions().then(function (result) {
        _this.divisions = result.Divisions;
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

        for (var _iterator2 = division.Players, _isArray2 = Array.isArray(_iterator2), _i2 = 0, _iterator2 = _isArray2 ? _iterator2 : _iterator2[Symbol.iterator]();;) {
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

          if (player.selected) playerIds.push(player.Id);
        }
      }

      this.api.createTeam({ name: this.name, email: this.email, playerIds: playerIds }).then(function (result) {
        console.info(result);
        _this2.status = 'Done!';
      });
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

      console.info('Player Selected: ' + player.LastName + ' : ' + player.selected);
      console.info('Div Selections: ' + division.selectedCount);
      console.info('Valid Team: ' + this.validLineup);
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

          if (division.selectedCount != division.PlayerRequirement) return false;
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

	var _desc, _value, _class, _descriptor, _descriptor2;

	var NavBar = exports.NavBar = (_class = function () {
		function NavBar() {
			_classCallCheck(this, NavBar);

			_initDefineProp(this, 'router', _descriptor, this);

			_initDefineProp(this, 'api', _descriptor2, this);

			this.updateStatus = '';
		}

		NavBar.prototype.attached = function attached() {
			var _this = this;

			this.getUpdateStatus();
			setInterval(function () {
				return _this.getUpdateStatus();
			}, 60000);
		};

		NavBar.prototype.getUpdateStatus = function getUpdateStatus() {
			var _this2 = this;

			this.api.getEvents('TeamUpdate').then(function (results) {
				if (results != null) {
					_this2.setUpdateStatus(results.LeagueEvents[0].Completed);
				}
			});
		};

		NavBar.prototype.setUpdateStatus = function setUpdateStatus(completed) {
			if (completed === undefined) {
				this.updateStatus = 'Stat update running now';
				return;
			}

			completed = (0, _moment2.default)(completed).toDate();

			this.updateStatus = 'Stats updated ' + (0, _moment2.default)(_moment2.default.utc([completed.getFullYear(), completed.getMonth(), completed.getDate(), completed.getHours(), completed.getMinutes(), completed.getSeconds()]).toDate()).fromNow();
		};

		return NavBar;
	}(), (_descriptor = _applyDecoratedDescriptor(_class.prototype, 'router', [_aureliaFramework.bindable], {
		enumerable: true,
		initializer: function initializer() {
			return null;
		}
	}), _descriptor2 = _applyDecoratedDescriptor(_class.prototype, 'api', [_aureliaFramework.bindable], {
		enumerable: true,
		initializer: function initializer() {
			return null;
		}
	})), _class);
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
define('text!app.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"bootstrap/css/bootstrap.css\"></require>\n  <require from=\"./styles.css\"></require>\n  <nav-bar router.bind=\"router\" api.bind=\"api\"></nav-bar>\n  <loading loading.bind=\"router.isNavigating || api.isRequesting\"></loading>\n  <div class=\"container\">\n    <router-view></router-view>\n  </div>\n</template>"; });
define('text!styles.css', ['module'], function(module) { module.exports = "body { padding-top: 70px; }\n\n.splash {\n  text-align: center;\n  margin: 10% 0 0 0;\n  box-sizing: border-box;\n}\n\n.splash .message {\n  font-size: 72px;\n  line-height: 72px;\n  text-shadow: rgba(0, 0, 0, 0.5) 0 0 15px;\n  text-transform: uppercase;\n  font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif;\n}\n\n.splash .fa-spinner {\n  text-align: center;\n  display: inline-block;\n  font-size: 72px;\n  margin-top: 50px;\n}\n\n.page-host {\n  position: absolute;\n  left: 0;\n  right: 0;\n  top: 50px;\n  bottom: 0;\n  overflow-x: hidden;\n  overflow-y: auto;\n}\n\n@media print {\n  .page-host {\n    position: absolute;\n    left: 10px;\n    right: 0;\n    top: 50px;\n    bottom: 0;\n    overflow-y: inherit;\n    overflow-x: inherit;\n  }\n}\n\nsection {\n  margin: 0 20px;\n}\n\n.navbar-nav li.loader {\n  margin: 12px 24px 0 6px;\n}\n\n/* animate page transitions */\nsection.au-enter-active {\n  -webkit-animation: fadeInRight 1s;\n  animation: fadeInRight 1s;\n}\n\ndiv.au-stagger {\n  /* 50ms will be applied between each successive enter operation */\n  -webkit-animation-delay: 50ms;\n  animation-delay: 50ms;\n}\n\n.card-container.au-enter {\n  opacity: 0;\n}\n\n.card-container.au-enter-active {\n  -webkit-animation: fadeIn 2s;\n  animation: fadeIn 2s;\n}\n\n.card {\n  overflow: hidden;\n  position: relative;\n  border: 1px solid #CCC;\n  border-radius: 8px;\n  text-align: center;\n  padding: 0;\n  background-color: #337ab7;\n  color: rgb(136, 172, 217);\n  margin-bottom: 32px;\n  box-shadow: 0 0 5px rgba(0, 0, 0, .5);\n}\n\n.card .content {\n  margin-top: 10px;\n}\n\n.card .content .name {\n  color: white;\n  text-shadow: 0 0 6px rgba(0, 0, 0, .5);\n  font-size: 18px;\n}\n\n.card .header-bg {\n  /* This stretches the canvas across the entire hero unit */\n  position: absolute;\n  top: 0;\n  left: 0;\n  width: 100%;\n  height: 70px;\n  border-bottom: 1px #FFF solid;\n  border-radius: 6px 6px 0 0;\n}\n\n.card .avatar {\n  position: relative;\n  margin-top: 15px;\n  z-index: 100;\n}\n\n.card .avatar img {\n  width: 100px;\n  height: 100px;\n  -webkit-border-radius: 50%;\n  -moz-border-radius: 50%;\n  border-radius: 50%;\n  border: 2px #FFF solid;\n}\n\n/* animation definitions */\n@-webkit-keyframes fadeInRight {\n  0% {\n    opacity: 0;\n    -webkit-transform: translate3d(100%, 0, 0);\n    transform: translate3d(100%, 0, 0)\n  }\n  100% {\n    opacity: 1;\n    -webkit-transform: none;\n    transform: none\n  }\n}\n\n@keyframes fadeInRight {\n  0% {\n    opacity: 0;\n    -webkit-transform: translate3d(100%, 0, 0);\n    -ms-transform: translate3d(100%, 0, 0);\n    transform: translate3d(100%, 0, 0)\n  }\n  100% {\n    opacity: 1;\n    -webkit-transform: none;\n    -ms-transform: none;\n    transform: none\n  }\n}\n\n@-webkit-keyframes fadeIn {\n  0% {\n    opacity: 0;\n  }\n  100% {\n    opacity: 1;\n  }\n}\n\n@keyframes fadeIn {\n  0% {\n    opacity: 0;\n  }\n  100% {\n    opacity: 1;\n  }\n}\n\n/* CARDS */\ndiv.card-container {\n  margin: 3rem;\n\tposition: relative;\n\twidth:248px;\n\theight:341px;\n  cursor: pointer;\n}\n\ndiv.card-frame {\n\tposition: absolute;\n\tleft: 0;\n\ttop: 0;\n\twidth:248px;\n\theight:341px;\t\n\tbackground: url(\"../assets/images/cardframe.png\");\n\tz-index: 1000;\n}\n\nimg.card-logo {\n\ttop: 20px;\n\tleft: 20px;\n  position: absolute;\n\twidth:50px;\n\theight:50px;\t\n\tz-index: 1000;\n}\n\nimg.card-picture {\n\ttop: 15px;\n\tleft: 21px;\n\tposition: absolute;\n\twidth:213px;\n\theight:320px;\t\n}\n\ndiv.card-nameplate {\n  display: flex;\n  justify-content:center;\n  align-items:center;\n\ttop: 295px;\n\tleft: 73px;\n\tposition: absolute;\n\tbackground-color:blue;\n\twidth:157px;\n\theight:34px;\n}\n\ndiv.card-nameplate span.playername {\n  text-transform: uppercase;\n  color: white;\n  font-weight: bold;\n  font-size: 0.9em;\n  padding: 0 3 0 3;\n}\n\nimg.img-player {\n  box-shadow: 3px 3px 1px #888888;\n}\n\n.list-row {\n  margin-top:20px;\n}\n\n.hr-count {\n  font-size: 36px; \n  border-bottom: 5px solid darkred; \n  margin-top: 15px; \n  display: inline-block;\n}"; });
define('text!pages/activity/activity.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"../../resources/value-converters/date-format\"></require>\n  <require from=\"../../resources/value-converters/home-away\"></require>\n  <require from=\"../../resources/value-converters/win-loss\"></require>\n  <div if.bind=\"recent.length === 0\">\n    <h1>There have not been any home runs hit in the past week.</h1>\n  </div>\n  <div repeat.for=\"stat of recent\">\n    <div class=\"list-row\">\n      <div if.bind=\"recent[$index-1].GameDate != stat.GameDate\" class=\"row\">\n        <div class=\"col-md-6 col-md-offset-3\">\n          <h1>\n            ${stat.GameDate | dateFormat:'dddd, MMMM Do'}\n          </h1>\n          <hr/>\n        </div>\n      </div>\n      <div class=\"row\">\n        <div class=\"col-md-1 col-md-offset-3\">\n          <div class=\"hr-count pull-right\">\n            ${stat.Hr}\n          </div>\n        </div>\n        <div class=\"col-md-1\"> \n          <img src=\"${stat.PlayerImage}\" class=\"img-player img-responsive\" />\n        </div>\n        <div class=\"col-md-6\">\n          <h1>${stat.DisplayName}</h1>\n          <p>In a ${stat.TeamScore} - ${stat.OppnentScore} ${stat.Result | winLoss} ${stat.HomeAway | homeAway} the ${stat.Opponent}</p>\n        </div>\n      </div>\n    </div>\n</template>"; });
define('text!pages/leaders/homeruns.html', ['module'], function(module) { module.exports = "<template>\n  <div repeat.for=\"player of leaders\" class=\"row\">\n    <div class=\"list-row\">\n      <div class=\"col-md-1 col-md-offset-3\">\n        <div class=\"hr-count pull-right\">\n          ${player.Hr}\n        </div>\n      </div>\n      <div class=\"col-md-1\">\n        <img src=\"${player.PlayerImage}\" class=\"img-player img-responsive\" />\n      </div>\n      <div class=\"col-md-6\">\n        <button click.delegate=\"toggleHistory(player)\" class=\"btn btn-link\" style=\"outline: none;\"><h1>${player.DisplayName}</h1></button>\n\n        <a if.bind=\"player.Hr7 >= 3\" class='my-tool-tip' data-toggle=\"tooltip\" data-placement=\"top\" title=\"3+ HRs in past 7 days.\">\n          <span  class=\"glyphicon glyphicon-fire\" style=\"color:#E25822\"></span>\n        </a>\n        \n        <a if.bind=\"player.Hr14 == 0\" class='my-tool-tip' data-toggle=\"tooltip\" data-placement=\"top\" title=\"No HRs in past 14 days.\">\n          <span  class=\"glyphicon glyphicon-asterisk\" style=\"color:#AAD4E5\"></span>\n        </a>\n        <div id=\"history-${player.PlayerId}\" style=\"display:none;\" class=\"col-md-8\">\n          <hr/>\n          <div class=\"pull-left\">\n            <div repeat.for=\"item of player.history.recentHr\">\n              <div>\n                <u>${item.date}</u> - <b>${item.hr}</b> ${item.location} ${item.opp}\n              </div>\n            </div>\n          </div>\n          <div class=\"pull-right\">\n            <div class=\"text-right\">\n              Last <u>7</u> Days: <b>${player.Hr7}</b>\n            </div>\n            <div class=\"text-right\">\n              Last <u>14</u> Days: <b>${player.Hr14}</b>\n            </div>\n            <div class=\"text-right\">\n              Last <u>30</u> Days: <b>${player.Hr30}</b>\n            </div>\n          </div>\n          <div class=\"clearfix\"></div>\n          <hr/>\n        </div>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/standings/list.html', ['module'], function(module) { module.exports = "<template>\n  <div repeat.for=\"team of teams\" class=\"row\">\n    <div class=\"list-row\">\n      <div class=\"col-md-1 col-md-offset-3\">\n        <div style=\"font-size: 36px; border-bottom: 5px solid darkred; margin-top: 15px; display: inline-block;\">\n          ${team.Totals.Hr}\n        </div>\n        <div>\n          <sup if.bind=\"team.Totals.HrMovement > 0\" class=\"text-success pull-right\" style=\"font-size: 15px; margin-top: -40px; margin-right: -8px;\">+${team.Totals.HrMovement}</sup>\n        </div>\n      </div>\n      <div class=\"col-md-7\">\n        <h1>${team.Name}</h1>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/teams/create.html', ['module'], function(module) { module.exports = "<template>\n  <require from=\"../../resources/elements/baseball-card\"></require>\n  <div class=\"row\">\n    <div class=\"col-md-10\">\n      <p class=\"lead\">\n        So you think you've got what it takes to dominate the Homerun League? It's time to prove it. You will need to construct a\n        valid team first. In order to do that, pick the required number of players from each division. After you have done\n        that, pick a team name (be creative!), fill out your email address, and confirm your lineup. Finally, all that there's\n        left to do is enjoy the baseball season and root for your team in the standings!\n      </p>\n      <div repeat.for=\"division of divisions\">\n        <h1>${division.Name}</h1>\n        <h2>Pick ${division.PlayerRequirement} players</h2>\n        <div repeat.for=\"player of division.Players\">\n          <div class=\"pull-left\">\n            <baseball-card id.bind=\"'player-card-' + player.Id\" player.bind=\"player\"></baseball-card>\n            <button disabled.bind=\"division.selectedCount >= division.PlayerRequirement && !player.selected\" class.bind=\"player.selected ? 'btn-success glyphicon-ok' : 'glyphicon-minus'\"\n              class=\"btn btn-default btn-lg glyphicon\" click.delegate=\"togglePlayer(division, player)\"></button>\n          </div>\n        </div>\n        <div class=\"clearfix\"></div>\n      </div>\n    </div>\n    <div class=\"col-md-2\">\n      <div id=\"myAffix\">\n        <div repeat.for=\"division of divisions\">\n          <div>${division.Name}: ${division.selectedCount == null ? '0' : division.selectedCount}/${division.PlayerRequirement}</div>\n        </div>\n        <form class=\"form-horizontal\">\n          <div class=\"form-group\">\n            <label class=\"sr-only\" for=\"teamName\">Team Name</label>\n            <input type=\"text\" class=\"form-control col-md-2\" id=\"teamName\" placeholder=\"Team Name\" value.bind=\"name\">\n          </div>\n          <div class=\"form-group\">\n            <label class=\"sr-only\" for=\"email\">Password</label>\n            <input type=\"email\" class=\"form-control col-md-2\" id=\"email\" placeholder=\"Email Address\" value.bind=\"email\">\n          </div>\n          <button disabled.bind=\"!validLineup\" click.trigger=\"createTeam()\"  type=\"submit\" class=\"btn btn-primary\">Create Team</button>\n        </form>\n        <div>\n          Team creation placeholder. ${validLineup}\n        </div>\n        <div>\n          ${status}\n        </div>\n      </div>\n    </div>\n  </div>\n</template>"; });
define('text!pages/welcome/index.html', ['module'], function(module) { module.exports = "<template>\n  <div class=\"teamname test\">Paul Konerko 1234567890</div>\n  <div class=\"teamname test2\">Jake Arrieta 1234567890</div>\n  <div class=\"teamname test3\">Ken Griffey Jr. 1234567890</div>\n</template>\n"; });
define('text!resources/elements/baseball-card.html', ['module'], function(module) { module.exports = "<template>\n  <div id.bind=\"id\" class=\"card-container\">\n    <div class=\"front\">\n        <div class=\"card-frame\"></div>\n        <img class=\"card-logo\" src=\"${player.TeamLogo2X}\" />\n        <img class=\"card-picture\" src=\"${player.PlayerImage}\" />\n        <div class=\"card-nameplate\">\n          <span class=\"playername\">\n            ${player.DisplayName}\n          </span>\n        </div>\n    </div>\n    <div class=\"back\">\n      <p>Back of ${player.DisplayName}'s baseball card!</p>\n      <img src=\"${player.TeamLogo2X}\"  />\n      \n    </div>\n  </div>\n</template>"; });
define('text!resources/elements/nav-bar.html', ['module'], function(module) { module.exports = "<template>\n\t<nav class=\"navbar-default navbar-fixed-top\">\n\t\t<div class=\"container\">\n\t\t\t<div class=\"navbar-header\">\n\t\t\t\t<button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#navbar-collapse\"\n\t\t\t\t\taria-expanded=\"false\">\n\t\t\t\t\t<span class=\"sr-only\">Toggle navigation</span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t\t<span class=\"icon-bar\"></span>\n\t\t\t\t</button>\n\t\t\t\t<a href=\"#\" class=\"navbar-brand\">Homerun League!</a>\n\t\t\t</div>\n\t\t\t<div class=\"collapse navbar-collapse\" id=\"navbar-collapse\">\n\t\t\t\t<ul class=\"nav navbar-nav\">\n\t\t\t\t\t<li repeat.for=\"row of router.navigation\" class=\"${row.isActive ? 'active' : ''}\">\n\t\t\t\t\t\t<a href.bind=\"row.href\">${row.title}</a>\n\t\t\t\t\t</li>\n\t\t\t\t</ul>\n\t\t\t\t<p class=\"navbar-text navbar-right\"><span class=\"text-info\">${updateStatus}</span></p>\n\t\t\t</div>\n\t\t</div>\n\t</nav>\n</template>"; });
//# sourceMappingURL=app-bundle.js.map