import * as nprogress from 'nprogress';
import {bindable, noView, decorators} from 'aurelia-framework';

export let Loading = decorators(
  noView(['nprogress/nprogress.css']),
  bindable({name: 'loading', defaultValue: false})
).on(class {
  loadingChanged(newValue){
    if (newValue) {
      nprogress.start();
    } else {
      nprogress.done();
    }
  }
});