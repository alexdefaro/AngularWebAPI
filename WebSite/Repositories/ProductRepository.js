'use strict';

(function() {

    angular.module( "application" ).service( "productRepository",  function( $http, toaster ) {
        
        var productRepositoryInstance = this; 

		productRepositoryInstance.deleteProduct = function ( productId ) {
            return $http.delete( "http://localhost:24322/api/products/" + productId );
		} 

		productRepositoryInstance.getProducts = function( currentPageNumber, numberOfRowsPerPage ) { 
            return $http.get( "http://localhost:24322/api/products/" + "?currentPageNumber="   + currentPageNumber 
                                                                     + "&numberOfRowsPerPage=" + numberOfRowsPerPage );   
		};
        
		productRepositoryInstance.getProduct = function( productId ) {
            return $http.get( "http://localhost:24322/api/products/" + productId ); 
		}; 

		productRepositoryInstance.insertProduct = function ( product ) {
            return $http.post( "http://localhost:24322/api/products", product );
        };

		productRepositoryInstance.updateProduct = function ( product ) {
            return $http.put( "http://localhost:24322/api/products/" + product.Id, product )
        }; 

    });
     
} )();

  