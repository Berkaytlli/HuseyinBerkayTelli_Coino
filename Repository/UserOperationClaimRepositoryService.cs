using AppEnvironment;
using Context;
using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserOperationClaimRepositoryService : BaseRepository<UserOperationClaim>, IUserOperationClaimRepositoryService
    {
        public UserOperationClaimRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
        public Result Remove(int claimId, int operatorUserId)
        {
            try
            {
                var requestExists = Get(u => u.OperationClaimId == claimId && u.isActive == true).Select(a => a.Id).ToList();
                foreach (var i in requestExists)
                {   
                    var claims = GetFirst(a => a.Id == i);
                    claims.Data.isActive = false;
                    Delete(claims.Data, operatorUserId);
                }

                return new Result<UserOperationClaim>();
            }
            catch (Exception e)
            {
                return new Result<UserOperationClaim>(e);
            }
        }
        public Result<UserOperationClaim> AddClaimToUserAtFirstRegister(int userId)
        {
            try
            {
                //var getRole = GetFirst(whereCondition: o => o.OperationClaim.Name == "User");
                //if (!getRole.IsSuccess)
                //    return new Result<UserOperationClaim>(getRole.MessageType ??
                //                                              MessageType.RecordNotFound);
                UserOperationClaim model = new UserOperationClaim
                {
                    OperationClaimId = 1,
                    UserId = userId,
                    isActive = true
                };
                var resultCreate = Add(model, null);

                //var getRole2 = GetFirst(o => o.OperationClaim.Name == "Attendee");
                //if (!getRole2.IsSuccess)
                //    return new Result<UserOperationClaim>(getRole2.MessageType ??
                //                                              MessageType.RecordNotFound);
                UserOperationClaim model1 = new UserOperationClaim
                {
                    OperationClaimId = 2,
                    UserId = userId,
                    isActive = true
                };
                var resultCreate1 = Add(model1, null);

                return resultCreate;
            }
            catch (Exception e)
            {
                return new Result<UserOperationClaim>(e);
            }
        }
    }
}
