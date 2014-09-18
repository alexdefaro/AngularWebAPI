'use strict';

(function () {

	var productModule = angular.module( "productModule", [] );
	 
	productModule.controller( 'productController', [ "$http", 
                                                     "productRepository", "categoryRepository",
                                                     "dialogService", "paginationService", 
                                                     function( $http, 
                                                               productRepository, categoryRepository,
                                                               dialogService, paginationService ) {
        var self = this; 

        self.productList  = []; 
        self.categoryList = []; 

		var deleteProduct = function ( productId ) {
            productRepository.deleteProduct( productId ) .success( function( resultData ) {
                                                                 self.resetView();
                                                             })
		} 

		var getAllCategories = function() {   
            categoryRepository.getAllCategories()
                              .success( function( resultData, status, headers ) { 
                                   self.categoryList = resultData;    
                               })
		}; 

        self.product     = {};
        self.productList = [];  

        this.resetView =  function() { 
            dialogService.clearAlertMessages();

            getAllCategories();

            self.product = {};
            self.getProducts();

            paginationService.setCurrentPageNumber( 1 ); 
            paginationService.setChangeMethod( this.getProducts );

        }

		this.getProduct = function( productId ) { 
            productRepository.getProduct( productId ).success( function( resultData ) {
                                                             self.product = resultData;  
                                                         })
		};

		this.getProducts = function() {   
            productRepository.getProducts( paginationService.Options.currentPageNumber, paginationService.Options.numberOfRowsPerPage )
                             .success( function( resultData, status, headers ) { 
                                   paginationService.setNumberOfRowsInResult( headers( "X-TotalRowCount" ) ); 
                                   self.productList = resultData;    
                               })
		}; 

		this.saveProduct = function ( product ) {

            product.DeactivationDate = ( product.Active == true ) ? null : new Date(); 

            if ( ( product.Id == undefined ) || ( product.Id == 0 ) ) {
                productRepository.insertProduct( product ).success( function( resultData ) {
                                                                  self.resetView();
                                                                  dialogService.showSuccess( "Succes", product.Name + " successfuly addded." );
                                                              }).error( function( resultData ) { 
                                                                  dialogService.showModelStateErros( resultData );
                                                              })
            } else {
                productRepository.updateProduct( product ).success( function( resultData ) {
                                                                  self.resetView();
                                                                  dialogService.showSuccess( "Succes", product.Name + " successfuly updated." );
                                                              }).error( function( resultData ) { 
                                                                  dialogService.showModelStateErros( resultData );
                                                              })
            }
		}

		this.confirmDelete = function( product ) {
            var modalResult = dialogService.show( "Confirm...",
                                                  "Are you sure you want to delete " + product.Name + "? " );
            modalResult.then( 
                function ( resultData ) {                         /* Success */
                    deleteProduct( product.Id );
                    dialogService.showNote( "Delete", product.Name + " successfuly deleted." );
                }, 
                function ( resultData ) {                         /* Error */
                    dialogService.showModelStateErros( resultData );
                }
            );
        }
         
	} ] )
     
    productModule.directive( "productsForm", function() {
        return {
            restrict : "E",
            templateUrl : "ProductsForm.html"
        };
    });

})();

