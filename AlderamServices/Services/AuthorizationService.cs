using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Providers.Entities;

namespace AlderamServices.Services
{
    public class AuthorizationService : IHttpModule  
    {
        public void Init(HttpApplication httpApplication)
        { 
            httpApplication.AuthenticateRequest += OnApplicationAuthenticateRequest;
            httpApplication.EndRequest          += OnApplicationEndRequest;
        }

        private static bool CheckPassword( string userName, string userPassword )
        {
            return userName == "alex" && userPassword == "alexpassword";
        }

        private static void SaveGenericIdentity( GenericIdentity genericIdentity ) 
        { 
            Thread.CurrentPrincipal = new GenericPrincipal( genericIdentity, null );  
                
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = Thread.CurrentPrincipal;
            }
        } 

        private static bool CheckLoginToken( string loginToken )
        {
            return loginToken == "alexalexpassword";
        }

        public static bool AuthenticateLoginToken( string loginToken )
        {
            if ( CheckLoginToken( loginToken ) )
            {
                SaveGenericIdentity( new GenericIdentity( loginToken ) );
                
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException( "Authentication failed." ); 
            }
        }

        public static bool AuthenticateUser( string userName, string userPassword )
        {
            if ( CheckPassword( userName, userPassword ) )
            {
                SaveGenericIdentity( new GenericIdentity( userName ) );

                return true;
            }
            else
            {
                throw new UnauthorizedAccessException( "Authentication failed." ); 
            }
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var currentRequest = HttpContext.Current.Request;
            var requestHeader  = currentRequest.Headers["Authorization"];

            if ( ! currentRequest.Path.Contains( "api" ) )  
            {
                HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.OK );
            }
            else if ( requestHeader == null ) 
            {
                HttpContext.Current.Response.StatusCode = Convert.ToInt32( HttpStatusCode.Unauthorized );
            }
            else
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse( requestHeader );

                if ( authenticationHeaderValue.Scheme.Equals( "Credentials", StringComparison.OrdinalIgnoreCase) && authenticationHeaderValue.Parameter != null)
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    string credentials = encoding.GetString( Convert.FromBase64String( authenticationHeaderValue.Parameter ) );

                    string[] credentialsInformation =  credentials.Split( ':' );
                    
                    AuthenticateUser( credentialsInformation[0].ToString(), credentialsInformation[1].ToString() );
                }
            }
        }

        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            
            if ( response.StatusCode == 401 )
            {
                response.Headers.Add( "WWW-Authenticate",string.Format("Authenticate=\"{0}\"", "Credentials" ) );
            }
        }

        public void Dispose()
        {
        }
    } 
}