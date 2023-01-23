using AppEnvironment;
using Entity.Authentication;
using ViewModel.Authentication;

namespace Business.UserBusinessService
{
    public interface IUserBusinessService
    {
        Result<User> ForgotPassword(ForgotPasswordVM model);
        //Result<User> VerifyEmailToken(string token);
        //Result<User> CreateEmailToken(EmailConfirmVM model);
        //Result<User> GetUserById(int id);
        //Result<User> GetUserByEmail(string userMail);
        //Result<User> EditPersonalInfo(int id, UserEditVM modelDTO, int operatorUserId);
    }
}
