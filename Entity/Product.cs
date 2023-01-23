using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public string Picture { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}