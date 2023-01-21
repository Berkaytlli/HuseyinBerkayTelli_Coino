using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class User
    {
        [Key]public int UserId { get; set; }
        public string Email { get; set; } =string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set;}
    }
}
