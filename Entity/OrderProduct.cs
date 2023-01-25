using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Authentication;

namespace Entity
{
    public class OrderProduct : BaseEntity 
    {
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; }
        public string OrderStatus { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Stock { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserAppId { get; set; }
        [ForeignKey("UserAppId")]
        public User User { get; set; }
    }
}
