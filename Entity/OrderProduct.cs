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
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderPrice { get; set; }
        public string OrderStatus { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string UserAppId { get; set; }
        [ForeignKey("UserAppId")]
        public User User { get; set; }
    }
}
