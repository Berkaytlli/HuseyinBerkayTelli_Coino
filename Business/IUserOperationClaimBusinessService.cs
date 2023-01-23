using AppEnvironment;
using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business
{
    public interface IUserOperationClaimBusinessService
    {
        public Result<UserOperationClaim> AddClaimToUser(UserOperationClaimVM vM, int operatorUserId);
        public Result RemoveClaimOfUser(UserOperationClaimVM vM, int operatorUserId);
        public Result<UserOperationClaim> AddClaimToUserAtFirstRegister(int userId);
        public Result<List<string>> GetClaimsOfChoosenUser(string userEmail);
    }
}
