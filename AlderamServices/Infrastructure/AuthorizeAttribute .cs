using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using System.Web.Providers.Entities;
using System.Net;
using System.Net.Http;
using System.Text;
using AlderamServices.Services;
 
namespace AlderamServices.Infrastructure
{
    public class AuthorizeAccess : AuthorizeAttribute
    {
        private DataBaseContext _DataBaseContext = new DataBaseContext();

        public AuthorizeAccess()
	    {

	    }

        private bool HasAllowAnonymousAttribute( HttpActionContext actionContext)
        {
            return ( ( actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ) || 
                     ( actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ) );
        }

        public override void OnAuthorization( HttpActionContext actionContext )
        { 
            if ( ! this.IsAuthorized( actionContext ) )
            {
                actionContext.Response = new HttpResponseMessage( HttpStatusCode.Unauthorized );
            } 
        }

        protected override bool IsAuthorized( HttpActionContext actionContext )
        {
            if ( HasAllowAnonymousAttribute( actionContext ) )
            { 
                HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.OK );
                return true;
            }

            if ( actionContext.Request.Headers.Authorization == null ) 
            {
                HttpContext.Current.Response.Headers.Add( "WWW-Authenticate",string.Format("Authenticate=\"{0}\"", "Credentials" ) );
                HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.Unauthorized );
                
                return false;
            }
            else
            {
                var authenticationHeaderValue = actionContext.Request.Headers.Authorization;

                if ( authenticationHeaderValue.Parameter != null)
                { 
                    try
                    {
                        if ( authenticationHeaderValue.Scheme.Equals( "Credentials", StringComparison.OrdinalIgnoreCase) )
                        {
                            var encoding = Encoding.GetEncoding("iso-8859-1");
                            string authenticationHeader = encoding.GetString( Convert.FromBase64String( authenticationHeaderValue.Parameter ) );

                            string[] authenticationHeaderformation =  authenticationHeader.Split( ':' );

                            AuthorizationService.AuthenticateUser( authenticationHeaderformation[0].ToString(), authenticationHeaderformation[1].ToString() );
                        } 
                        else if ( authenticationHeaderValue.Scheme.Equals( "Token", StringComparison.OrdinalIgnoreCase) ) {
                        
                            AuthorizationService.AuthenticateLoginToken( authenticationHeaderValue.Parameter.ToString() );
                        }

                        HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.OK );
                        return true;
                    }
                    catch (Exception)
                    {
                        HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.Unauthorized );
                        return false;
                    }                    
                }
            }
            
            HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.Unauthorized );
            return false;
        }

    }
}