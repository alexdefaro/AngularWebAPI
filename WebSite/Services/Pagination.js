'use strict';

(function() {

    angular.module( "application" ).service( "paginationService",  function() {

        var paginationServiceInstance = this;

        paginationServiceInstance.Options = {
            numberOfRowsInResult : 0,
            numberOfRowsPerPage  : 10,
            currentPageNumber    : 1,
            changeMethod         : null
        }

        paginationServiceInstance.setNumberOfRowsInResult = function( numberOfRowsInResult ) {
            paginationServiceInstance.Options.numberOfRowsInResult = parseInt( numberOfRowsInResult ); 
        }

        paginationServiceInstance.setnumberOfRowsPerPage  = function( numberOfRowsPerPage ) {
            paginationServiceInstance.Options.numberOfRowsPerPage = parseInt( numberOfRowsPerPage ); 
        }

        paginationServiceInstance.setCurrentPageNumber    = function( currentPageNumber ) {
            paginationServiceInstance.Options.currentPageNumber = parseInt( currentPageNumber ); 
        }

        paginationServiceInstance.setChangeMethod    = function( changeMethod ) {
            paginationServiceInstance.Options.changeMethod = changeMethod; 
        } 
    });

	angular.module( "application" ).controller( "paginationController", function( paginationService ) {

        var paginationControllerInstance = this;

        paginationControllerInstance.Options = paginationService.Options;

        paginationControllerInstance.resetPaginator = function() {
            paginationService.setCurrentPageNumber( 1 );
        }
    });          
     
    angular.module( "application" ).directive( "paginationControl", function() {
        return {
            restrict : "E",
            templateUrl : "/Views/Directives/paginationControl.html",
            link: function ( scope, elm, attrs ) { 
            }
        };
    });   

} )();
 