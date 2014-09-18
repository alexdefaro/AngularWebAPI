using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlderamServices.Infrastructure;
using AlderamServices.Models;
using System.Web.Http.Cors;

namespace AlderamServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        private DataBaseContext _DataBaseContext = new DataBaseContext(); 

        public HomeController()
        {
            SaveCategories();
            SaveProducts();
        }

        private void SaveCategories()
        {
            _DataBaseContext.Categories.ToList().ForEach( R => _DataBaseContext.Categories.Remove(R) );

            for (int I = 1; I < 50; I++)
            {
                Category categoryI = new Category() { Id = I, Code = I.ToString(), Name = "Category "+I.ToString(), DeactivationDate = null };
                _DataBaseContext.Categories.Add( categoryI );
            }

            _DataBaseContext.SaveChanges();
        }

        private void SaveProducts()
        {
            _DataBaseContext.Products.ToList().ForEach(R => _DataBaseContext.Products.Remove(R));

            //Product product1category1 = new Product() {Id = 1, Name = "Product 1", Code = "1", Category = _DataBaseContext.Categories.Single( R => R.Id == 1 )};
            //Product product2category1 = new Product() {Id = 2, Name = "Product 2", Code = "2", Category = _DataBaseContext.Categories.Single( R => R.Id == 1 )};
            //Product product3category2 = new Product() {Id = 3, Name = "Product 3", Code = "3", Category = _DataBaseContext.Categories.Single( R => R.Id == 2 )};
            //Product product4category2 = new Product() {Id = 4, Name = "Product 4", Code = "2", Category = _DataBaseContext.Categories.Single( R => R.Id == 2 )};

            Product product1category1 = new Product() {Id = 1, Name = "Product 1", Code = "1", CategoryId = 1 };
            Product product2category1 = new Product() {Id = 2, Name = "Product 2", Code = "2", CategoryId = 1 };
            Product product3category2 = new Product() {Id = 3, Name = "Product 3", Code = "3", CategoryId = 2 };
            Product product4category2 = new Product() {Id = 4, Name = "Product 4", Code = "2", CategoryId = 2 };

            _DataBaseContext.Products.Add(product1category1);
            _DataBaseContext.Products.Add(product2category1);
            _DataBaseContext.Products.Add(product3category2);
            _DataBaseContext.Products.Add(product4category2);

            _DataBaseContext.SaveChanges();
        }

        [HttpGet] [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
