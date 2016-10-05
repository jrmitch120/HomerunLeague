export class BaseballCardIdValueConverter {
  toView(value) {
    return value.toString().substring(0,3);
  }
}