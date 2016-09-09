import {bindable} from 'aurelia-framework'
import $ from 'bootstrap';

export class BaseballCard {
    @bindable id = '';
    @bindable player = {};

    flipCard() {
        console.info(this.id);
    }
}