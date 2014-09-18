using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace AlderamServices.Models
{
    public class Category
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Description("Category ID")]
        public int Id { get; set; }

        [Description("Category code")] [Required(ErrorMessage="Category code is required")]
        public string Code  { get; set; }
        
        [Description("Category name")] [Required(ErrorMessage="Category name is required")]
        public string Name  { get; set; }

        [Description("Category active status")]
        public bool Active { 
                             get { return ( this.DeactivationDate == null ); }  
                             set { if ( value == true ) { this.DeactivationDate = null; }
                                   else { this.DeactivationDate =  System.DateTime.Now; } } 
                           }
        
        [Description("Category deactivation date")]
        public DateTime? DeactivationDate { get; set; }

        //[Description("Category associated products ")] [IgnoreDataMember]
        //public IList<Product> Products { get; set; }
        
        public Category()
        {
            //this.Products = new List<Product>();
        }
    }
}