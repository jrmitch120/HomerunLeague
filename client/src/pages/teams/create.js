import {inject} from 'aurelia-framework';
import {Api} from '../../services/api';
import $ from 'bootstrap';
import bootstrap from 'bootstrap';

@inject(Api)
export class Create {
	constructor (api) {
    this.api = api;
    this.divisions = [];
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
    return this.api.getDivisions().then(result => {this.divisions = result.Divisions;});                               
  }
  
  get validLineup() {
    for (var division of this.divisions) {
      if(division.selectedCount != division.PlayerRequirement)
            return false;
    }
      
    return true;
  }
  
  togglePlayer(division, player) 
  {  
    if(division.selectedCount == null)
      division.selectedCount = 0;
      
    if(player.selected) {
      player.selected = false; // Works!
      division.selectedCount--;
    }
    else {
      player.selected = true; // Works!
      division.selectedCount++;
    }
    
    console.info(`Player Selected: ${player.LastName} : ${player.selected}`);
    console.info(`Div Selections: ${division.selectedCount}`);
    console.info(`Valid Team: ${this.validLineup}`);
  }
}   
