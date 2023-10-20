using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Duration { get; set; }
        public decimal Price { get; set; }
        public List<Category> Categories { get; set; }
    }
}
