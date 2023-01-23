using AppEnvironment;
using Authentication.JWTs;

namespace Business;

public interface IAuthBusinessService
{
    Result<AccessToken> Login(string email, string password, bool isBackofficeLogIn = false);
    Result<AccessToken> Register(string firstname, string lastname, string email, string phone, string password, string address);
    Result<AccessToken> CreateNewAccessToken(int id);
}