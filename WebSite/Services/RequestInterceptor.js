'use strict';

angular.module("application").factory( "httpInterceptor",
    function( $q, $rootScope, $location ) {
        return {
            request: function(config) {
                return config||$q.when(config);
            },
            requestError: function(request) {
                return $q.reject(request);
            },
            response: function(response) {
                return response||$q.when(response);
            },
            responseError: function(response) { 
                if( response&&response.status === 404 ) {
                
                } 
                else if(response&&response.status >= 401) {
                     
                }
                else if(response&&response.status >= 500) {
                
                }
                return $q.reject(response);
            }
        }
    } 
);