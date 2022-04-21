var application = angular.module('IARestaurantApp', ['ngAnimate', 'toaster']);

application.constant('DOMAIN', {
  BASE_URL: 'http://localhost:5166/api',
  cache: false
});
application.filter('formatId', () => {
  function getFormatId ( number, width = 6 ){
    width -= number.toString().length;
    if ( width > 0 )
    {
      return new Array( width + (/\./.test( number ) ? 2 : 1) ).join( '0' ) + number;
    }
    return number + ""; // siempre devuelve tipo cadena
  }
  return  (value, length) => {
    return getFormatId((value, length));
  };
});