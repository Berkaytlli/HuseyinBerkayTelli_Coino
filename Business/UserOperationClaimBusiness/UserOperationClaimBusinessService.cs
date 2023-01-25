using AppEnvironment;
using Context;
using Entity.Authentication;
using Repository.OperationClaimRepository;
using Repository.UserOperationClaimRepository;
using Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.UserOperationClaimBusiness
{
    public class UserOperationClaimBusinessService : IUserOperationClaimBusinessService
    {
        private readonly IUserOperationClaimRepositoryService _userOperation;
        private readonly IUserRepositoryService _userRepositoryService;
        private readonly IOperationClaimRepositoryService _operationClaimRepositoryService;
        private readonly ApplicationDbContext _dbContext;

        public UserOperationClaimBusinessService(IUserOperationClaimRepositoryService userOperation,
                                                  IUserRepositoryService userBusiness,
                                                  IOperationClaimRepositoryService operationClaim,
                                                  ApplicationDbContext dbContext)
        {
            _userOperation = userOperation;
            _userRepositoryService = userBusiness;
            _operationClaimRepositoryService = operationClaim;
            _dbContext = dbContext;
        }

        public Result<UserOperationClaim> AddClaimToUser(UserOperationClaimVM vM, int operatorUserId)
        {
            try
            {
                var getUser = _userRepositoryService.GetFirst(u => u.Email == vM.UserEmail);
                if (!getUser.IsSuccess)
                    return new Result<UserOperationClaim>(getUser.MessageType ??
                                                              MessageType.RecordNotFound);
                var getRole = _operationClaimRepositoryService.GetFirst(o => o.Name == vM.ClaimName);
                if (!getRole.IsSuccess)
                    return new Result<UserOperationClaim>(getRole.MessageType ?? MessageType.RecordNotFound);
                UserOperationClaim model = new UserOperationClaim
                {
                    OperationClaimId = getRole.Data.Id,
                    UserId = getUser.Data.Id,
                    isActive = true
                };
                var getIsActive = _userOperation.GetFirst(u =>
                u.OperationClaimId == model.OperationClaimId && u.UserId == model.UserId && u.isActive == false);
                if (getIsActive.IsSuccess)
                {
                    getIsActive.Data.isActive = true;
                    var updateIsActive = _userOperation.Update(getIsActive.Data, operatorUserId);
                }
                using var transaction = _dbContext.Database.BeginTransaction();
                var requestExists = _userOperation.GetFirst(u =>
                u.OperationClaimId == model.OperationClaimId && u.UserId == model.UserId && u.isActive == true);
                if (requestExists.IsSuccess)
                    return new Result<UserOperationClaim>(requestExists.MessageType ?? MessageType.RecordAlreadyExists);
                var resultCreate = _userOperation.Add(model, operatorUserId);
                if (!resultCreate.IsSuccess)
                    return new Result<UserOperationClaim>(resultCreate.MessageType ?? MessageType.InsertFailed);

                transaction.Commit();
                return resultCreate;
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
                var getRole = _operationClaimRepositoryService.GetFirst(o => o.Name == "User");
                if (!getRole.IsSuccess)
                    return new Result<UserOperationClaim>(getRole.MessageType ??
                                                              MessageType.RecordNotFound);
                UserOperationClaim model = new UserOperationClaim
                {
                    OperationClaimId = getRole.Data.Id,
                    UserId = userId,
                    isActive = true
                };
                var resultCreate = _userOperation.Add(model, null);

                var getRole2 = _operationClaimRepositoryService.GetFirst(o => o.Name == "Attendee");
                if (!getRole2.IsSuccess)
                    return new Result<UserOperationClaim>(getRole2.MessageType ??
                                                              MessageType.RecordNotFound);
                UserOperationClaim model1 = new UserOperationClaim
                {
                    OperationClaimId = getRole2.Data.Id,
                    UserId = userId,
                    isActive = true
                };
                var resultCreate1 = _userOperation.Add(model1, null);

                return resultCreate;
            }
            catch (Exception e)
            {
                return new Result<UserOperationClaim>(e);
            }
        }

        public Result<List<string>> GetClaimsOfChoosenUser(string userEmail)
        {
            try
            {
                List<string> userOperationClaims = new List<string>();
                var getUser = _userRepositoryService.GetFirst(u => u.Email == userEmail);
                if (!getUser.IsSuccess)
                    return new Result<List<string>>(MessageType.RecordNotFound);
                var requestExists = _userOperation.Get(u => u.UserId == getUser.Data.Id && u.isActive == true).Where(r => r.OperationClaim.DeletedAt == null).Select(a => a.OperationClaim.Id).ToArray();
                foreach (var i in requestExists)
                {
                    var claims = _operationClaimRepositoryService.GetFirst(o => o.Id == i);
                    userOperationClaims.Add(claims.Data.Name);
                }

                return new Result<List<string>>(userOperationClaims);
            }
            catch (Exception e)
            {
                return new Result<List<string>>(e);
            }
        }

        public Result RemoveClaimOfUser(UserOperationClaimVM vM, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getUser = _userRepositoryService.GetFirst(u => u.Email == vM.UserEmail);
                if (!getUser.IsSuccess)
                    return new Result<UserOperationClaim>(getUser.MessageType ??
                                                                MessageType.RecordNotFound);
                var getRoles = _operationClaimRepositoryService.GetFirst(o => o.Name == vM.ClaimName);
                if (!getRoles.IsSuccess)
                    return new Result<UserOperationClaim>(getRoles.MessageType ??
                                                                MessageType.RecordNotFound);
                var getClaim = _userOperation.GetFirst(u => u.UserId == getUser.Data.Id && u.OperationClaimId == getRoles.Data.Id && u.isActive == true);
                if (!getClaim.IsSuccess)
                    return new Result(getUser.MessageType ?? MessageType.RecordNotFound);
                getClaim.Data.isActive = false;
                var result = _userOperation.Delete(getClaim.Data, operatorUserId);
                if (!result.IsSuccess)
                    return new Result<UserOperationClaim>(result.MessageType ??
                                                              MessageType.DeleteFailed);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<UserOperationClaim>(e);
            }
        }
    }
}
