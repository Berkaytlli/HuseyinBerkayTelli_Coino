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
    public interface IOperationClaimBusinessService
    {
        Result<OperationClaim> CreateClaim(OperationClaimVM vM, int operatorUserId);
        Result<OperationClaim> EditClaim(string claimName, OperationClaimVM vM, int operatorUserId);
        Result RemoveClaim(string claimName, int operatorUserId);
        Result<OperationClaim> GetClaimByName(string claimName);
        Result<OperationClaim> GetClaimById(int id);
        Result<List<OperationClaim>> GetClaims();
    }
}
