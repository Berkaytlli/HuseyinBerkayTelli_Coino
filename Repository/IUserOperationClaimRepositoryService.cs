using AppEnvironment;
using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserOperationClaimRepositoryService : IBaseRepositoryService<UserOperationClaim>
    {
        public Result Remove(int claimId, int operatorUserId);
        public Result<UserOperationClaim> AddClaimToUserAtFirstRegister(int userId);
    }
}
