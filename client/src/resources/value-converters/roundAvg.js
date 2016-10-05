export class RoundAvgValueConverter {
  toView(value, places) {
    return value.toFixed(places).toString().substring(1);
  }
}