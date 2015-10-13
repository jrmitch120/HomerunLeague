import {inject} from 'aurelia-framework';
import {PlayerData} from '../data/playerData';

@inject(PlayerData)
export class Create {
	constructor (playerData) {
		
    this.data = playerData;
  }
  
  activate(params) {
    return this.data.getPlayer(1).then(players => this.players = players);                               
  }
}
