using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ProductVM
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Stock { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}
