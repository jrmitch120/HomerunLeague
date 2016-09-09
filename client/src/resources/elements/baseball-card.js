import {bindable} from 'aurelia-framework';
import $ from 'bootstrap';
import 'flip';

export class BaseballCard {
    @bindable id = '';
    @bindable player = {};

    attached() {
        //console.info(this.id);
        $(`#${this.id}`).flip({trigger: 'click'}); 
    }

    flipCard() {

        console.info(this.id);
    }
}