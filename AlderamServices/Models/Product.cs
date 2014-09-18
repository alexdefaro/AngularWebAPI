using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace AlderamServices.Models
{
    public class Product
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Description("Category ID")]
        public int Id { get; set; }

        [Description("Product name")] [Required(ErrorMessage="Product name is required")]
        public string Name { get; set; } 

        [Description("Product code")] [Required(ErrorMessage="Product code is required")]
        public string Code  { get; set; }

        [Description("Category Id")] 
        public int CategoryId { get; set; }

        [Description("Category")] 
        public Category Category { get; set; }
        
        [Description("Category active status")]
        public bool Active { 
                             get { return ( this.DeactivationDate == null ); }  
                             set { if ( value == true ) { this.DeactivationDate = null; }
                                   else { this.DeactivationDate =  System.DateTime.Now; } } 
                           }
        
        [Description("Category deactivation date")]
        public DateTime? DeactivationDate { get; set; }

        public Product()
        {

        }
    }
}