using AppEnvironment;
using Authentication.Extensions;
using Authentication.Hashing;
using Context;
using Entity.Authentication;
using GenerateKey.OTP;
using Microsoft.AspNetCore.Http;
using Repository;
using System.Security.Claims;
using ViewModel.Authentication;

namespace Business.UserBusinessService
{
    public class UserBusinessService : IUserBusinessService
    {
        private readonly IUserRepositoryService _userRepositoryService;
        private readonly IOtpHelper _otpHelper;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserBusinessService(IUserRepositoryService userRepositoryService,
                                   IOtpHelper otpHelper,
                                   ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userRepositoryService = userRepositoryService;
            _dbContext = dbContext;
            _otpHelper = otpHelper;
            _httpContextAccessor = httpContextAccessor;
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
        //public Result ChangePassword(string currentPassword, string newPassword)
        //{
        //    try
        //    {
        //        var user = GetCurrentUser();
        //        if (user == null) return new Result(MessageType.Unauthorized);

        //        if (!HashingHelper.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
        //            return new Result(MessageType.IncorrectPassword);

        //        HashingHelper.CreatePasswordHash(newPassword, out var passwordHash, out var passwordSalt);

        //        user.PasswordHash = passwordHash;
        //        user.PasswordSalt = passwordSalt;

        //        _userRepositoryService.Update(user, user.Id);

        //        return new Result(MessageType.UpdateSuccess);
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(e);
        //    }
        //}



        //public int GetCurrentUser()
        //{
        //    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    return Convert.ToInt32(userId);
        //}
    }

    //    public Result<User> VerifyEmailToken(string token)
    //    {
    //        using var transaction = _dbContext.Database.BeginTransaction();
    //        try
    //        {
    //            var getToken = _userRepositoryService.GetFirst(u => u.VerifyMailToken == token && u.MailVerifiedAt == null);
    //            if (!getToken.IsSuccess)
    //                return new Result<User>(getToken.MessageType ?? MessageType.RecordNotFound);
    //            getToken.Data.MailVerifiedAt = DateTime.Now;
    //            int operatorUserId = getToken.Data.Id;
    //            var verifyToken = _userRepositoryService.Update(getToken.Data, operatorUserId);
    //            if (!verifyToken.IsSuccess)
    //                return new Result<User>(verifyToken.MessageType ?? MessageType.UpdateFailed);
    //            transaction.Commit();
    //            return new Result<User>(verifyToken.MessageType ?? MessageType.OperationSuccess);
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }
    //    public Result<User> PhoneConfirmation(PhoneConfirmVM model)
    //    {
    //        try
    //        {
    //            var getUser = _userRepositoryService.GetFirst(x => x.Phone == model.PhoneNumber && x.PhoneVerifiedAt == null);
    //            if (!getUser.IsSuccess)
    //                return new Result<User>(getUser.MessageType ?? MessageType.RecordNotFound);

    //            var setKey = _otpHelper.CreateOtpKey(6, getUser.Data.Id);
    //            if (!setKey.IsSuccess)
    //                return new Result<User>(setKey.MessageType ?? MessageType.OperationFailed);

    //            string fullName = getUser.Data.FirstName + " " + getUser.Data.LastName;
    //            var sendKey = _smsService.SendSmsService(model.PhoneNumber, userName: fullName, key: setKey.Data.GeneratedKey, date: setKey.Data.OtpKeyExpiration.ToString(), language: getUser.Data.Language);
    //            if (!sendKey.IsSuccess)
    //                return new Result<User>(sendKey.MessageType ?? MessageType.OperationFailed);

    //            return new Result<User>(sendKey.MessageType ?? MessageType.OperationSuccess);
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }

    //    public Result<User> ConfirmPhoneToken(string token, int operatorUserId)
    //    {
    //        try
    //        {
    //            var getUser = _userRepositoryService.GetFirst(x => x.Id == operatorUserId && x.PhoneVerifiedAt == null);
    //            if (!getUser.IsSuccess)
    //                return new Result<User>(getUser.MessageType ?? MessageType.RecordNotFound);
    //            var confirmResult = _otpHelper.ValidateOtpKey(token, operatorUserId);
    //            if (!confirmResult.IsSuccess)
    //                return new Result<User>(confirmResult.MessageType ?? MessageType.OperationFailed);
    //            using var transaction = _dbContext.Database.BeginTransaction();
    //            getUser.Data.PhoneVerifiedAt = DateTime.Now;
    //            var resultConfirm = _userRepositoryService.Update(getUser.Data, operatorUserId);
    //            if (!resultConfirm.IsSuccess)
    //                return new Result<User>(confirmResult.MessageType ?? MessageType.UpdateFailed);
    //            transaction.Commit();
    //            return new Result<User>(resultConfirm.MessageType ?? MessageType.OperationSuccess);
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }

    //    public Result<User> CreateEmailToken(EmailConfirmVM model)
    //    {
    //        using var transaction = _dbContext.Database.BeginTransaction();
    //        try
    //        {
    //            var getUser = _userRepositoryService.GetFirst(x => x.Email == model.EmailAdress && x.MailVerifiedAt == null);
    //            if (!getUser.IsSuccess)
    //                return new Result<User>(getUser.MessageType ?? MessageType.RecordNotFound);
    //            var createToken = _userRepositoryService.CreateVerifyToken(getUser.Data);
    //            if (!createToken.IsSuccess)
    //                return new Result<User>(createToken.MessageType ?? MessageType.OperationFailed);
    //            string verifyToken = createToken.Data.Token;
    //            getUser.Data.VerifyMailToken = verifyToken;
    //            getUser.Data.MailTokenExpiration = createToken.Data.Expiration;
    //            var addTokenToUser = _userRepositoryService.Update(getUser.Data, getUser.Data.Id);
    //            if (!addTokenToUser.IsSuccess)
    //                return new Result<User>(addTokenToUser.MessageType ?? MessageType.UpdateFailed);
    //            string fullName = getUser.Data.FirstName + " " + getUser.Data.LastName;
    //            string[] recipients = new[] { getUser.Data.Email };
    //            var sendEmail = _emailService.SendRegisterEmail(language: getUser.Data.Language,
    //                recipients: recipients,
    //                key: verifyToken,
    //                userName: fullName
    //                );
    //            if (!sendEmail.IsSuccess)
    //                return new Result<User>(sendEmail.MessageType ?? MessageType.OperationFailed);

    //            transaction.Commit();
    //            return new Result<User>(MessageType.OperationSuccess);
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(MessageType.OperationFailed);
    //        }
    //    }
    //    public Result<User> GetUserById(int id)
    //    {
    //        try
    //        {
    //            var requestExists = _userRepositoryService.GetFirst(u => u.Id == id);
    //            if (!requestExists.IsSuccess)
    //                return new Result<User>(requestExists.MessageType ?? MessageType.RecordNotFound);

    //            return requestExists;
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }

    //    public Result<User> EditPersonalInfo(int id, UserEditVM modelDTO, int operatorUserId)
    //    {
    //        using var transaction = _dbContext.Database.BeginTransaction();

    //        try
    //        {
    //            var requestExists = GetUserById(id);
    //            requestExists.Data.FirstName = modelDTO.Name;
    //            requestExists.Data.LastName = modelDTO.SurName;
    //            requestExists.Data.Email = modelDTO.Email;
    //            requestExists.Data.UserImage = modelDTO.UserImage;
    //            requestExists.Data.Phone = modelDTO.PhoneNumber;
    //            requestExists.Data.DateOfBirth = modelDTO.DateOfBirth;
    //            var resultEdit = _userRepositoryService.Update(requestExists.Data, operatorUserId);
    //            if (!resultEdit.IsSuccess)
    //                return new Result<User>(resultEdit.MessageType ?? MessageType.UpdateFailed);
    //            transaction.Commit();
    //            return resultEdit;
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }

    //    public Result<User> GetUserByEmail(string userMail)
    //    {
    //        try
    //        {
    //            var requestExists = _userRepositoryService.GetFirst(u => u.Email == userMail);
    //            if (!requestExists.IsSuccess)
    //                return new Result<User>(requestExists.MessageType ?? MessageType.RecordNotFound);

    //            return requestExists;
    //        }
    //        catch (Exception e)
    //        {
    //            return new Result<User>(e);
    //        }
    //    }


