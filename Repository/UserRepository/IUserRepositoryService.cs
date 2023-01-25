using AppEnvironment;
using Authentication.JWTs;
using Entity.Authentication;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppEnvironment.Enums.EnumBase;

namespace Repository.UserRepository
{
    public interface IUserRepositoryService : IBaseRepositoryService<User>
    {
        public Result<User> Register(string firstname, string lastname, string email, string phone, string password, string address);
        public Result<User> Login(string identifier, string password, LoginVia loginVia = LoginVia.all);
        public Result<AccessToken> CreateAccessToken(User user);


    }
}
