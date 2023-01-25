using AppEnvironment;
using Authentication.JWTs;
using Context;
using Entity.Authentication;
using Repository.UserOperationClaimRepository;
using Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AuthBusiness;

public class AuthBusinessService : IAuthBusinessService
{
    private readonly IUserRepositoryService _userRepositoryService;
    private readonly IUserOperationClaimRepositoryService _userOperation;
    private readonly ApplicationDbContext _dbContext;
    public AuthBusinessService(IUserRepositoryService userRepositoryService,
                               ApplicationDbContext dbContext,
                               IUserOperationClaimRepositoryService userOperation)
    {
        _userRepositoryService = userRepositoryService;
        _dbContext = dbContext;
        _userOperation = userOperation;
    }

    public Result<AccessToken> CreateNewAccessToken(int id)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var getUser = _userRepositoryService.GetFirst(whereCondition: c => c.Id == id);
            if (!getUser.IsSuccess) return new Result<AccessToken>(MessageType.Forbidden);
            var result = _userRepositoryService.CreateAccessToken(getUser.Data);
            transaction.Commit();
            return result;
        }
        catch (Exception e)
        {
            return new Result<AccessToken>(e);
        }
    }
    public Result<AccessToken> Login(string email, string password, bool isBackofficeLogIn = false)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var userToLogin = _userRepositoryService.Login(email, password);
            if (isBackofficeLogIn)
            {
                //todo
            }

            if (!userToLogin.IsSuccess) return new Result<AccessToken>(userToLogin.MessageType ?? MessageType.RecordNotFound);

            var result = _userRepositoryService.CreateAccessToken(userToLogin.Data);
            _userRepositoryService.Update(userToLogin.Data, userToLogin.Data.Id);
            transaction.Commit();
            return result;
        }
        catch (Exception e)
        {
            return new Result<AccessToken>(e);
        }
    }
    public Result<AccessToken> Register(string firstname, string lastname, string email, string phone, string password, string address)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var userExists = _userRepositoryService.GetFirst(u => u.Email == email);
            if (userExists.IsSuccess)
                return new Result<AccessToken>(userExists.MessageType ?? MessageType.RecordAlreadyExists);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname)
            || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
            {
                return new Result<AccessToken>(MessageType.OperationFailed);
            }
            var registerResult = _userRepositoryService.Register(
                email: email, password: password, firstname: firstname, lastname: lastname, phone: phone, address: address);
            if (!registerResult.IsSuccess)
                return new Result<AccessToken>(registerResult.MessageType ?? MessageType.OperationFailed) { Message = registerResult.Message };

            AddInialClaim(registerResult.Data.Id);

            var result = _userRepositoryService.CreateAccessToken(registerResult.Data);

            transaction.Commit();
            return result.IsSuccess ? result : new Result<AccessToken>(result.MessageType ?? MessageType.OperationFailed) { Data = result.Data, Message = result.Message };
        }
        catch (Exception e)
        {
            return new Result<AccessToken>(e);
        }
    }

    private Result<UserOperationClaim> AddInialClaim(int id)
    {
        var giveRole = _userOperation.AddClaimToUserAtFirstRegister(id);

        if (!giveRole.IsSuccess)
            return new Result<UserOperationClaim>(giveRole.MessageType ?? MessageType.ExceptionOccurred);
        else
            return new Result<UserOperationClaim>(giveRole.Data);

    }


}
