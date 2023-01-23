using AppEnvironment;
using Authenticaion.JWT;
using Authentication.Hashing;
using Authentication.JWTs;
using Context;
using Entity.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppEnvironment.Enums.EnumBase;

namespace Repository
{
    public class UserRepositoryService : BaseRepository<User>, IUserRepositoryService
    {
        private readonly IJwtHelper _tokenHelper;

        public UserRepositoryService(ApplicationDbContext context, IJwtHelper tokenHelper) : base(context)
        {
            _tokenHelper = tokenHelper;
        }

        public Result<User> Register(string firstname, string lastname, string email, string phone, string password, string address)
        {
            try
            {
                HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                var user = new User
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                    Phone = phone,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Address = address
                    
                };
                Add(user, null);
                return new Result<User>(user);
            }
            catch (Exception e)
            {
                return new Result<User>(e);
            }
        }
        public Result<User> Login(string identifier, string password, LoginVia loginVia = LoginVia.all)
        {
            try
            {
                var formattedPhone = identifier; //todo 
                var userToCheckResult = loginVia switch
                {
                    LoginVia.email => GetFirst(u => u.Email == identifier),
                    LoginVia.all => GetFirst(u => u.Email == identifier),
                    
                    _ => throw new ArgumentOutOfRangeException(nameof(loginVia), loginVia, null)
                };

                if (!userToCheckResult.IsSuccess)
                {
                    return new Result<User>(MessageType.RecordNotFound);
                }

                var userToCheck = userToCheckResult.Data;
                //if (!HashingHelper.VerifyPasswordHash(password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
                //{
                //    return new Result<User>(MessageType.OperationFailed)
                //    {
                //        IsSuccess = false,
                //        Message = "Wrong password"
                //    };
                //}

                return new Result<User>(userToCheck);
            }
            catch (Exception e)
            {
                return new Result<User>(e);
            }
        }

        public Result<AccessToken> CreateAccessToken(User user)
        {
            try
            {
                var userWithClaimsResult = GetFirst(
                    makeJoins: set => set
                        .Include(u => u.OperationClaims)
                        .ThenInclude(oc => oc.OperationClaim),
                    whereCondition: u =>
                        u.Id == user.Id
                );
                if (!userWithClaimsResult.IsSuccess)
                {
                    return new Result<AccessToken>(MessageType.OperationFailed)
                    {
                        Message = userWithClaimsResult.Message
                    };
                }
                var claims = userWithClaimsResult.Data.OperationClaims.Where(u => u.isActive == true).Select(u => u.OperationClaim).Where(u => u.DeletedAt == null);
                var accessToken = _tokenHelper.CreateToken(user, claims);
                return new Result<AccessToken>(accessToken);
            }
            catch (Exception e)
            {
                return new Result<AccessToken>(e);
            }
        }
       
    }
}
