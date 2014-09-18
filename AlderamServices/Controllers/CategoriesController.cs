using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using AlderamServices.Models;
using AlderamServices.Infrastructure;
using System.Web.Http.Cors; 

namespace AlderamServices.Controllers
{
    [AuthorizeAccess] [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-TotalRowCount")]

    public class CategoriesController : ApiController
    {
        private DataBaseContext _DataBaseContext = new DataBaseContext();

        [HttpGet] [AllowAnonymous] [Route("api/allcategories/")]

        public IEnumerable<Category> GetAllCategories()
        {
            var queryStatement = _DataBaseContext.Categories.AsQueryable();
            int numberOfRecordsInQuery = queryStatement.Count();
            
            HttpContext.Current.Response.Headers.Add( "X-TotalRowCount", numberOfRecordsInQuery.ToString() );

            var queryResult = queryStatement.AsEnumerable();
            
            return queryResult;
        }

        [HttpGet] [AllowAnonymous]
        public IEnumerable<Category> GetCategories( int currentPageNumber = 1, int numberOfRowsPerPage = 20 )
        {
            int skipRows = ( numberOfRowsPerPage * ( currentPageNumber - 1 ) ); 
            
            var queryStatement = _DataBaseContext.Categories.AsQueryable();
            int numberOfRecordsInQuery = queryStatement.Count();
            
            HttpContext.Current.Response.Headers.Add( "X-TotalRowCount", numberOfRecordsInQuery.ToString() );

            var queryResult = queryStatement.OrderBy( o => o.Name ).Skip( skipRows ).Take( numberOfRowsPerPage ).AsEnumerable();
            
            return queryResult;
        }
        
        [HttpGet] 
        public Category GetCategory(int id)
        {
            Category category = _DataBaseContext.Categories.Find(id);
            
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return category;
        }
        
        [HttpGet] [Route("api/categories/{categoryId}/products")]
        public IEnumerable<Product> GetProducts(int categoryId)
        {
            return _DataBaseContext.Products.Where(R => R.Category.Id == categoryId).AsEnumerable();
        }
         
        [HttpPut]
        public HttpResponseMessage PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != category.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            _DataBaseContext.Entry(category).State = EntityState.Modified;

            try
            {
                _DataBaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage PostCategory(Category category)
        {
            if ( ModelState.IsValid )
            {
                _DataBaseContext.Categories.Add( category );
                _DataBaseContext.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.Created, category );
                response.Headers.Location = new Uri( Url.Link( "DefaultApi", new { id = category.Id } ) );
                return response;
            }
            else
            {
                return Request.CreateErrorResponse( HttpStatusCode.BadRequest, ModelState );
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCategory(int id)
        {
            Category category = _DataBaseContext.Categories.Find(id);
            if (category == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _DataBaseContext.Categories.Remove(category);

            try
            {
                _DataBaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        protected override void Dispose(bool disposing)
        {
            _DataBaseContext.Dispose();
            base.Dispose(disposing);
        }
    }
}