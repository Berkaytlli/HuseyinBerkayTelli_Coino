using AppEnvironment;
using Authentication.JWTs;
using Entity.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppEnvironment.Enums.EnumBase;

namespace Repository
{
    public interface IUserRepositoryService : IBaseRepositoryService<User>
    {
        public Result<User> Register(string email, string password);
        public Result<User> Login(string identifier, string password, LoginVia loginVia = LoginVia.all);
        public Result<AccessToken> CreateAccessToken(User user);
        

    }
}
