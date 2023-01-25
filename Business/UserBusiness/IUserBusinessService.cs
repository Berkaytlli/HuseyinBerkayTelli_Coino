using AppEnvironment;
using Entity.Authentication;
using System.Security.Claims;
using ViewModel.Authentication;

namespace Business.UserBusiness
{
    public interface IUserBusinessService
    {
        Result<User> ChangePassword(ChangePasswordVM model, ClaimsPrincipal claimsPrincipal);


    }
}
