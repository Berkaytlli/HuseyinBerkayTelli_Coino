using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Product : BaseEntity
    {
        
        public string ProductName { get; set; }

        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal Stock { get; set; }
       
        public string ProductCode { get; set; }
        public string? Image { get; set; }
        
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}