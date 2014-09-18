using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;

using AlderamServices.Infrastructure;
using AlderamServices.Models.ViewModels;
using AlderamServices.Services;
using System.Web.Http.Cors;

namespace AlderamServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticationController : ApiController
    {
        private AuthorizationService _AuthorizationService;

        public AuthenticationController()
        {
            _AuthorizationService = new AuthorizationService();
        }

        [HttpPost] 
        [Route("api/AuthenticateUser")]
        public HttpResponseMessage AuthenticateUser( LoginViewModel loginInformation )  
        {
            try
            {
                AuthorizationService.AuthenticateUser( loginInformation.UserName, loginInformation.UserPassword ); 
                return Request.CreateResponse( HttpStatusCode.OK, "User authenticated." );
            }
            catch (Exception)
            {
                return Request.CreateResponse( HttpStatusCode.Unauthorized, "Authentication failed." );
            }
        }
        
        [HttpPost] 
        [Route("api/AuthenticateToken")]
        public HttpResponseMessage AuthenticateToken( LoginTokenViewModel loginTokenViewModel )  
        {
            try
            {
                AuthorizationService.AuthenticateLoginToken( loginTokenViewModel.Token ); 
                return Request.CreateResponse( HttpStatusCode.OK, "User authenticated." );
            }
            catch (Exception)
            {
                return Request.CreateResponse( HttpStatusCode.Unauthorized, "Authentication failed." );
            }
        }

    }
}
