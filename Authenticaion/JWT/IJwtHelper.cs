using Authentication.JWTs;
using Entity.Authentication;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticaion.JWT
{
    public interface IJwtHelper
    {
        AccessToken CreateToken(User user, IEnumerable<Entity.Authentication.OperationClaim> operationClaims);
        AccessToken CreateRefreshToken(User user, IEnumerable<Entity.Authentication.OperationClaim> operationClaims);
        public JwtSecurityToken? ValidateJwtToken(string token);
    }
}
