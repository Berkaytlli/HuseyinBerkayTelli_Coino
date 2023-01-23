using AppEnvironment;
using Context;
using Entity.Authentication;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business
{
    public class OperationClaimBusinessService : IOperationClaimBusinessService
    {
        private readonly IOperationClaimRepositoryService _claimRepositoryService;
        private readonly IUserOperationClaimRepositoryService _userOperationClaimRepository;
        private readonly ApplicationDbContext _dbContext;
        public OperationClaimBusinessService(IOperationClaimRepositoryService claimRepositoryService,
                                               IUserOperationClaimRepositoryService userOperationClaimRepository,
                                                ApplicationDbContext dbContext)
        {
            _claimRepositoryService = claimRepositoryService;
            _userOperationClaimRepository = userOperationClaimRepository;
            _dbContext = dbContext;
        }

        public Result<OperationClaim> CreateClaim(OperationClaimVM vM, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                OperationClaim model = new OperationClaim
                {
                    Name = vM.ClaimName
                };
                var requestExists = _claimRepositoryService.GetFirst(u => u.Name == vM.ClaimName && u.DeletedAt == null);
                if (requestExists.IsSuccess)
                    return new Result<OperationClaim>(requestExists.MessageType ??
                                                              MessageType.RecordAlreadyExists);

                var resultCreate = _claimRepositoryService.Add(model, operatorUserId);
                if (!resultCreate.IsSuccess)
                    return new Result<OperationClaim>(resultCreate.MessageType ??
                                                              MessageType.InsertFailed);

                transaction.Commit();
                return resultCreate;
            }
            catch (Exception e)
            {
                return new Result<OperationClaim>(e);
            }
        }
        public Result<OperationClaim> EditClaim(string claimName, OperationClaimVM vM, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var currentClaim = _claimRepositoryService.GetFirst(u => u.Name == claimName && u.DeletedAt == null);
                if (!currentClaim.IsSuccess)
                    return new Result<OperationClaim>(currentClaim.MessageType ??
                                                                MessageType.RecordNotFound);

                currentClaim.Data.Name = vM.ClaimName;
                var resultEdit = _claimRepositoryService.Update(currentClaim.Data, operatorUserId);
                if (!resultEdit.IsSuccess)
                    return new Result<OperationClaim>(resultEdit.MessageType ??
                                                              MessageType.UpdateFailed);
                transaction.Commit();
                return resultEdit;
            }
            catch (Exception e)
            {
                return new Result<OperationClaim>(e);
            }
        }

        public Result<OperationClaim> GetClaimById(int id)
        {
            try
            {
                var requestExists = _claimRepositoryService.GetFirst(u => u.Id == id && u.DeletedAt == null);
                if (!requestExists.IsSuccess)
                    return new Result<OperationClaim>(requestExists.MessageType ?? MessageType.RecordNotFound);

                return requestExists;
            }
            catch (Exception e)
            {
                return new Result<OperationClaim>(e);
            }
        }

        public Result<OperationClaim> GetClaimByName(string claimName)
        {
            try
            {
                var requestExists = _claimRepositoryService.GetFirst(u => u.Name == claimName && u.DeletedAt == null);
                if (!requestExists.IsSuccess)
                    return new Result<OperationClaim>(requestExists.MessageType ?? MessageType.RecordNotFound);

                return requestExists;
            }
            catch (Exception e)
            {
                return new Result<OperationClaim>(e);
            }
        }

        public Result<List<OperationClaim>> GetClaims()
        {
            try
            {
                var result = _claimRepositoryService.Get(whereCondition: c => c.CreatedAt != null).ToList();
                if (!result.Any())
                    return new Result<List<OperationClaim>>(MessageType.RecordNotFound);

                return new Result<List<OperationClaim>>(result);
            }
            catch (Exception e)
            {
                return new Result<List<OperationClaim>>(e);
            }
        }

        public Result RemoveClaim(string claimName, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var currentClaim = GetClaimByName(claimName);
                if (!currentClaim.IsSuccess)
                    return new Result<OperationClaim>(currentClaim.MessageType ??
                                                                MessageType.RecordNotFound);

                currentClaim.Data.DeletedBy = operatorUserId;
                var result = _claimRepositoryService.Delete(currentClaim.Data, operatorUserId);
                if (!result.IsSuccess)
                    return new Result<OperationClaim>(result.MessageType ??
                                                              MessageType.DeleteFailed);
                var deleteFromUserOperationClaims = _userOperationClaimRepository.Remove(currentClaim.Data.Id, operatorUserId);
                transaction.Commit();

                return result;
            }
            catch (Exception e)
            {
                return new Result<OperationClaim>(e);
            }
        }
    }
}
