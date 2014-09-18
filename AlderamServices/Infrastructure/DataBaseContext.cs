using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using AlderamServices.Models;
using System.Diagnostics;

namespace AlderamServices.Infrastructure
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base()
        {
            #if DEBUG
             Database.Log = s => Debug.Write(s);
            #endif

            Database.SetInitializer(new DropCreateDatabaseAlways<DataBaseContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
             //base.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}