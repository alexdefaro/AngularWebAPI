angular.module( "application" ).factory( "applicationStart", function ( $http, $cookies ) {
  return {
      init: function ( tokenInformation ) {
          $http.defaults.headers.common.Authorization = "Token " + tokenInformation  
      }
  };
});