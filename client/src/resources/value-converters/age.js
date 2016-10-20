import moment from 'moment';

export class AgeValueConverter {
  toView(value) {
    return moment().diff(moment(value),'years');
  }
}