'use strict';

(function() {

    angular.module( "application" ).service( "dialogService",  function( $modal, toaster ) {

        var dialogServiceInstance = this;
        var dialogDefaultTimer = 3000;
        
        var parseModelStateErrors = function ( resultData ) {
            var errorMessages = [];

            for (var errorMessage in resultData.ModelState ) {
                for ( var i = 0; i < resultData.ModelState[errorMessage].length; i++) {
                    errorMessages.push( resultData.ModelState[errorMessage][i] );
                }
            }
            return errorMessages;
        }
         
        /* Alert  */
        dialogServiceInstance.alertMessages = [];

        dialogServiceInstance.showModelStateErros = function( modelState ) {  
            
            var errorMessages = parseModelStateErrors( modelState );

            dialogServiceInstance.clearAlertMessages();

            for ( var i = 0; i < errorMessages.length; i++) {
                dialogServiceInstance.alertMessages.push( errorMessages[i] );
            } 
        };

        dialogServiceInstance.clearAlertMessages = function() {  
            dialogServiceInstance.alertMessages.length = 0
        } 
        
        /* Gritter */
        dialogServiceInstance.showGritter = function( dialogType, dialogHeader, dialogText, dialogTimer, dialogTemplate ) { 
             toaster.pop( dialogType, dialogHeader, dialogText, dialogTimer, dialogTemplate );   
         };

        dialogServiceInstance.showSuccess = function( dialogHeader, dialogText ) { 
             dialogServiceInstance.showGritter( "success", dialogHeader, dialogText, dialogDefaultTimer );   
         };

        dialogServiceInstance.showError = function( dialogHeader, dialogText ) { 
             dialogServiceInstance.showGritter( "error", dialogHeader, dialogText, dialogDefaultTimer );   
         };

        dialogServiceInstance.showWarning = function( dialogHeader, dialogText ) { 
            dialogServiceInstance.showGritter( "warning", dialogHeader, dialogText, dialogDefaultTimer );   
         };

        dialogServiceInstance.showWait = function( dialogHeader, dialogText ) { 
            dialogServiceInstance.showGritter( "wait", dialogHeader, dialogText, dialogDefaultTimer );   
         };

        dialogServiceInstance.showNote = function( dialogHeader, dialogText ) { 
            dialogServiceInstance.showGritter( "note", dialogHeader, dialogText, dialogDefaultTimer );   
         }; 

        /* Dialogs */
        dialogServiceInstance.show = function( dialogHeader, dialogText ) { 
            
            dialogServiceInstance.dialogHeader = dialogHeader;
            dialogServiceInstance.dialogText   = dialogText;             

            var modalTemplate = {
                backdrop    : true,
                keyboard    : true,
                modalFade   : true,
                templateUrl : "Views/Dialogs/ConfirmDialog.html" 
            };
 
            dialogServiceInstance.dialogInstance = $modal.open( modalTemplate );

            return dialogServiceInstance.dialogInstance.result;
        };
    });

	angular.module( "application" ).controller( "dialogController", function( dialogService ) {

        this.dialogHeader  = dialogService.dialogHeader;  
        this.dialogText    = dialogService.dialogText;  
        this.alertMessages = dialogService.alertMessages;
         
        this.confirm = function() {
            dialogService.dialogInstance.close();
        }

        this.cancel = function() { 
            dialogService.dialogInstance.dismiss( "cancel" );
        } 

        this.closeAlertWindow = function() {  
            this.alertMessages.splice( 0, this.alertMessages.length );  
        } 
    });          
     
    angular.module( "application" ).directive( "alertControl", function() {
        return {
            restrict : "E",
            templateUrl : "/Views/Directives/alertControl.html",
             link: function ( scope, elm, attrs ) { 
             }
        };
    });   

    angular.module( "application" ).directive( "notifyControl", function() {
        return {
            restrict : "E",
            templateUrl : "/Views/Directives/notifyControl.html",
             link: function ( scope, elm, attrs ) { 
             }
        };
    });   

} )();

