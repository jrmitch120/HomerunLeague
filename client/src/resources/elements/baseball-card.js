import {bindable} from 'aurelia-framework';
import $ from 'bootstrap';
import 'flip';

export class BaseballCard {
    @bindable id = '';
    @bindable player = {};

    attached() {
        $(`#${this.id}`).flip({trigger: 'click'}); 
    }
}