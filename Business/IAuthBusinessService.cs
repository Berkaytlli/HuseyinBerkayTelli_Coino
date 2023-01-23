using AppEnvironment;
using Authentication.JWTs;

namespace Business;

public interface IAuthBusinessService
{
    Result<AccessToken> Login(string email, string password, bool isBackofficeLogIn = false);
    Result<AccessToken> Register(string email, string password);
    Result<AccessToken> CreateNewAccessToken(int id);
}