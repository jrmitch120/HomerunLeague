export class HomeAwayValueConverter {
  toView(value) {
    return value.toUpperCase() === 'H' ? 'vs' : 'at';
  }
}