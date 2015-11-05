import {inject} from 'aurelia-framework';
import {DivisionData} from '../data/divisionData';

@inject(DivisionData)
export class Create {
	constructor (divisionData) {
    this.data = divisionData;
  }
  
  activate(params) {
    return this.data.getDivisions().then(result => this.divisions = result.Divisions);                               
  }
  
  togglePlayer(division, player) {
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
    
    console.info(`Player selected: ${player.LastName} : ${player.selected}`);
    console.info(`Div selections: ${division.selectedCount}`);
  }
}
