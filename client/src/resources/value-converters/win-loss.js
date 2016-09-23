export class WinLossValueConverter {
  toView(value) {
    return value.toUpperCase() === 'W' ? 'win' : 'loss';
  }
}