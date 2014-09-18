using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AlderamServices.Controllers;
using AlderamServices.Models;

namespace AlderamServices.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            CategoriesController controller = new CategoriesController();
            IEnumerable<Category> result = controller.GetCategories();

            Assert.IsNotNull(result); 
        }

        [TestMethod]
        public void GetById()
        {
            CategoriesController controller = new CategoriesController();
            Category result = controller.GetCategory(5);

            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            CategoriesController controller = new CategoriesController();
            Category category = new Category(){ Id = 1 };
            controller.PostCategory(category);
        }

        [TestMethod]
        public void Put()
        {
            CategoriesController controller = new CategoriesController();
            Category category = new Category(){ Id = 1 };
            controller.PutCategory(1, category); 
        }

        [TestMethod]
        public void Delete()
        {
            CategoriesController controller = new CategoriesController();
            controller.DeleteCategory(1);
        }
    }
}
