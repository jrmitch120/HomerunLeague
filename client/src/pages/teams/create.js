import { inject } from 'aurelia-framework';
import { Api } from '../../services/api';
import $ from 'bootstrap';
import bootstrap from 'bootstrap';

@inject(Api)
export class Create {

  _api;
  name = '';
  email = '';
  password = '';
  status = '';
  divisions = [];
  saving = false;

  constructor(api) {
    this._api = api;
  }

  attached() {
    // Grab the affixed element.
    var element = $('#myAffix');

    // Enable affix
    element.affix();

    // Fixes an affix width bug.  This sets the width to the parent's width
    element.width(element.parent().width());
  }

  activate(params) {
    return this._api.getDivisions().then(result => { this.divisions = result.divisions; });
  }

  get validLineup() {
    for (var division of this.divisions) {
      if (division.selectedCount != division.playerRequirement)
        return false;
    }

    return true;
  }

  createTeam() {
    this.status = 'Saving...';
    this.saving = true;

    let playerIds = [];

    for (var division of this.divisions) {
      for (var player of division.players)
        if (player.selected)
          playerIds.push(player.id);
    }

    this._api.createTeam({ name: this.name, email: this.email, password: this.password, playerIds: playerIds }).then(result => {
      if (result.team !== undefined) {
        this.status = 'Your team has been entered.  Good luck!';
        this._reset();
      }
      else
        this.status = result.responseStatus.message;

      this.saving = false;
    }, error => {
      this.status = error;
      this.saving = false;
    });
  }

  loadPlayerStats(player) {
    if (player.playerTotals === undefined) {
      return this._api.getPlayer(player.id).then(result => {
        player.playerTotals = result.player.playerTotals;
      });
    }
    else
      return true;
  }

  scrollToAnchor(anchorName) {
    var aTag = $("a[name='" + anchorName + "']");
    $('html,body').animate({ scrollTop: aTag.offset().top - 55 }, 'slow');
  }

  togglePlayer(division, player) {
    if (division.selectedCount == null)
      division.selectedCount = 0;

    if (player.selected) {
      player.selected = false; // Works!
      division.selectedCount--;
    }
    else {
      player.selected = true; // Works!
      division.selectedCount++;
    }
  }

  _reset() {
    this.name = '';
    this.email = '';
    this.password = '';

    for (var division of this.divisions) {
      for (var player of division.players)
        if(player.selected)
          this.togglePlayer(division, player);
    }
  }
}   
