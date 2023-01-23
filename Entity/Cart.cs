using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Authentication;

namespace Entity
{
    public class Cart : BaseEntity
    {
        public Cart()
        {
            Count= 1;
        }
        public string UserAppId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        [ForeignKey("UserAppId")]
        public User User { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
