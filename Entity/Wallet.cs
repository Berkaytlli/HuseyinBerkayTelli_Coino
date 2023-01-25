using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Wallet : BaseEntity
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

}
