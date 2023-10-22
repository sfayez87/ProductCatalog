using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Category")]
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
