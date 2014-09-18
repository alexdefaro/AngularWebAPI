'use strict';

(function () {

	var categoryModule = angular.module( "categoryModule", ["ui.bootstrap"]  );
 
	categoryModule.controller( 'categoryController', [ "$http", 
                                                       "categoryRepository", 
                                                       "dialogService", "paginationService",
                                                       function( $http, 
                                                                 categoryRepository, 
                                                                 dialogService, paginationService ) {
        var self = this;

		var deleteCategory = function ( categoryId ) {
            categoryRepository.deleteCategory( categoryId ) .success( function( resultData ) {
                                                                 self.resetView();
                                                             })
		} 

        self.category     = {};
        self.categoryList = [];  

        this.resetView =  function() { 
            dialogService.clearAlertMessages();

            self.category = {};
            self.getCategories();

            paginationService.setCurrentPageNumber( 1 ); 
            paginationService.setChangeMethod( this.getCategories );
        }

		this.getCategory = function( categoryId ) { 
            categoryRepository.getCategory( categoryId ).success( function( resultData ) {
                                                             self.category = resultData;  
                                                         })
		};

		this.getCategories = function() {   
            categoryRepository.getCategories( paginationService.Options.currentPageNumber, paginationService.Options.numberOfRowsPerPage )
                              .success( function( resultData, status, headers ) { 
                                   paginationService.setNumberOfRowsInResult( headers( "X-TotalRowCount" ) );
                                   self.categoryList = resultData;    
                               })
		}; 

		this.saveCategory = function ( category ) {

            category.DeactivationDate = ( category.Active == true ) ? null : new Date();

            if ( ( category.Id == undefined ) || ( category.Id == 0 ) ) {
                categoryRepository.insertCategory( category ).success( function( resultData ) {
                                                                  self.resetView();
                                                                  dialogService.showSuccess( "Succes", category.Name + " successfuly addded." );
                                                              }).error( function( resultData ) { 
                                                                  dialogService.showModelStateErros( resultData );
                                                              })
            } else {
                categoryRepository.updateCategory( category ).success( function( resultData ) {
                                                                  self.resetView();
                                                                  dialogService.showSuccess( "Succes", category.Name + " successfuly updated." );
                                                              }).error( function( resultData ) { 
                                                                  dialogService.showModelStateErros( resultData );
                                                              })
            }
		}

		this.confirmDelete = function( category ) {
            var modalResult = dialogService.show( "Confirm...",
                                                  "Are you sure you want to delete " + category.Name + "? " );
            modalResult.then( 
                function ( resultData ) {                         /* Success */
                    deleteCategory( category.Id );
                    dialogService.showNote( "Delete", category.Name + " successfuly deleted." );
                }, 
                function ( resultData ) {                         /* Error */
                    dialogService.showModelStateErros( resultData );
                }
            );
        }

	} ] ) 
     
    categoryModule.directive( "categoriesForm", function() {
        return {
            restrict : "E",
            templateUrl : "CategoriesForm.html"
        };
    });      

} )();

