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
    public class ProductsController : ApiController
    {
        private DataBaseContext _DataBaseContext = new DataBaseContext();

        [AllowAnonymous]
        public IEnumerable<Product> GetProducts( int currentPageNumber = 1, int numberOfRowsPerPage = 20 )
        {
            int skipRows = ( numberOfRowsPerPage * ( currentPageNumber - 1 ) ); 
            
            var queryStatement = _DataBaseContext.Products.Include("Category").AsQueryable();
            int numberOfRecordsInQuery = queryStatement.Count();
            
            HttpContext.Current.Response.Headers.Add( "X-TotalRowCount", numberOfRecordsInQuery.ToString() );

            var queryResult = queryStatement.OrderBy( o => o.Name ).Skip( skipRows ).Take( numberOfRowsPerPage ).AsEnumerable();
            
            return queryResult;
        }         
        public Product GetProduct(int id)
        {
            Product product = _DataBaseContext.Products.Include("Category").FirstOrDefault( p => p.Id == id );
            
            if (product == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            } 

            return product;
        }

        public HttpResponseMessage PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != product.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
            
             product.Category = _DataBaseContext.Categories.Find( product.CategoryId );
            _DataBaseContext.Entry(product).State = EntityState.Modified;
            //_DataBaseContext.Entry(product).Reference( x => x.Category ).Load();

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

        public HttpResponseMessage PostProduct(Product product)
        {
            if ( product.Category != null )
            { 
                product.Category = _DataBaseContext.Categories.Find( product.Category.Id );
                ModelState.Remove( "product.Category.Code");
                ModelState.Remove( "product.Category.Name");
            }

            if (ModelState.IsValid)
            {
                _DataBaseContext.Products.Add(product);
                _DataBaseContext.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, product);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage DeleteProduct(int id)
        {
            if ( ! User.IsInRole( "Administrators" ) )
            { 
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            }

            Product product = _DataBaseContext.Products.Find(id);

            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _DataBaseContext.Products.Remove(product);

            try
            {
                _DataBaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        protected override void Dispose(bool disposing)
        {
            _DataBaseContext.Dispose();
            base.Dispose(disposing);
        }
    }
}