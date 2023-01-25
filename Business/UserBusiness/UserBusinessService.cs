using AppEnvironment;
using Authentication.Extensions;
using Authentication.Hashing;
using Context;
using Entity.Authentication;
using GenerateKey.OTP;
using Microsoft.AspNetCore.Http;
using Repository.UserRepository;
using System.Security.Claims;
using ViewModel.Authentication;

namespace Business.UserBusiness
{
    public class UserBusinessService : IUserBusinessService
    {
        private readonly IUserRepositoryService _userRepositoryService;
        private readonly ApplicationDbContext _dbContext;


        public UserBusinessService(IUserRepositoryService userRepositoryService,
                           ApplicationDbContext dbContext)
        {
            _userRepositoryService = userRepositoryService;
            _dbContext = dbContext;
        }

        public Result<User> ChangePassword(ChangePasswordVM model, ClaimsPrincipal claimsPrincipal)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                
                var userId = claimsPrincipal.GetUserId();
                var getUser = _userRepositoryService.GetFirst(x => x.Id == userId);
                if (!getUser.IsSuccess)
                    return new Result<User>(getUser.MessageType ?? MessageType.RecordNotFound);

               
                if (!HashingHelper.VerifyPasswordHash(model.OldPassword, getUser.Data.PasswordHash, getUser.Data.PasswordSalt))
                    return new Result<User>(MessageType.OperationFailed);

               
                HashingHelper.CreatePasswordHash(model.NewPassword, out var passwordHash, out var passwordSalt);

                
                getUser.Data.PasswordHash = passwordHash;
                getUser.Data.PasswordSalt = passwordSalt;
                getUser.Data.RefreshToken = null;
                getUser.Data.RefreshTokenExpires = DateTime.Now;

               
                var updatedPassword = _userRepositoryService.Update(getUser.Data, getUser.Data.Id);
                if (!updatedPassword.IsSuccess)
                    return new Result<User>(updatedPassword.MessageType ?? MessageType.UpdateFailed);

                
                transaction.Commit();
                return updatedPassword;
            }
            catch (Exception e)
            {
                return new Result<User>(e);
            }
        }
    }

}


