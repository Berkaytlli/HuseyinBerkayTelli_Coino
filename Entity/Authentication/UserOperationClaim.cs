using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Authentication
{
    public class UserOperationClaim : BaseEntity
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public bool isActive { get; set; }
        public virtual User User { get; set; }
        public OperationClaim OperationClaim { get; set; }
    }
}
