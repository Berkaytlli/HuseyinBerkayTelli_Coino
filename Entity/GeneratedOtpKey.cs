using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GeneratedOtpKey : BaseEntity
    {
        public string GeneratedKey { get; set; }
        public DateTime OtpKeyExpiration { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
