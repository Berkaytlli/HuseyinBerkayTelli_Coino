using Context;
using Entity.Authentication;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OperationClaimRepository
{
    public class OperationClaimRepositoryService : BaseRepository<OperationClaim>, IOperationClaimRepositoryService
    {
        public OperationClaimRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
    }
}
