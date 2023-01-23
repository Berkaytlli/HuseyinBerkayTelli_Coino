using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OrderDetail
    {
        
        public int OrderProductId { get; set; }
        [ForeignKey("OrderProductId")]
        public OrderProduct OrderProduct { get; set; }

    }
}
