'use strict';

(function() {

    angular.module( "application" ).service( "categoryRepository",  function( $http, toaster ) {
        
        var categoryRepositoryInstance = this; 

		categoryRepositoryInstance.deleteCategory = function ( categoryId ) {
            return $http.delete( "http://localhost:24322/api/categories/" + categoryId );
		} 

		categoryRepositoryInstance.getCategories = function( currentPageNumber, numberOfRowsPerPage ) { 
            return $http.get( "http://localhost:24322/api/categories/" + "?currentPageNumber="   + currentPageNumber 
                                                                       + "&numberOfRowsPerPage=" + numberOfRowsPerPage );   
		};
        
		categoryRepositoryInstance.getAllCategories = function() {  
            return $http.get( "http://localhost:24322/api/allcategories/" );   
		};

		categoryRepositoryInstance.getCategory = function( categoryId ) {
            return $http.get( "http://localhost:24322/api/categories/" + categoryId ); 
		}; 

		categoryRepositoryInstance.insertCategory = function ( category ) {
            return $http.post( "http://localhost:24322/api/categories", category );
        };

		categoryRepositoryInstance.updateCategory = function ( category ) {
            return $http.put( "http://localhost:24322/api/categories/" + category.Id, category )
        }; 

    });
     
} )();

  