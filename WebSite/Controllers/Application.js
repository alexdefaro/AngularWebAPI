(function() {

	var application = angular.module( "application", ["ngRoute", "ngResource", "ngCookies", "toaster",
                                                      "categoryModule", "productModule"] );
	
	var views = [ "Home", "Categories", "Products" ];  

    /* Application configuration */
    application.config( function( $routeProvider, $locationProvider, $httpProvider ) {

        /* Requests interceptor */
        $httpProvider.interceptors.push( "httpInterceptor" );    
          
        /* Routing configuration */
		$routeProvider
			          .when("/Categories", {
				          templateUrl : "Views/CategoriesForm.html" 
			          })
			          .when("/Products", {
				          templateUrl : "Views/ProductsForm.html" 
			          });

        /* set HTML5 rout mode */
        $locationProvider.html5Mode( true );
	});

    /* Controllers */
	application.controller( 'menuController', function() {
		
        this.activeViewIndex = 0;  
        this.valor = "";  

		this.setActiveView = function( selectedViewIndex ) {
			this.activeViewIndex = views.indexOf( selectedViewIndex );		 
		}	

		this.isActiveView = function( selectedViewIndex ) {
			return ( this.activeViewIndex == views.indexOf( selectedViewIndex ) );	
		}	
	} );

    application.run(function( applicationStart ) {
        applicationStart.init( "alexalexpassword" ); 
    });  


    //**//

    application.service( 'viewService', function( ){
        this.text = 'viewService text';    
    });

    application.controller( 'serviceController', function( viewService ){
        this.text = viewService.text;
        //viewService.text = 'serviceController text';    
    });

    application.controller( 'viewController', function( viewService ){
        viewService.text = 'viewController text';    
        this.text = viewService.text;
    }); 

} )();

