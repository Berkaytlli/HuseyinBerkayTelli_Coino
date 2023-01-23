using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticaion.JWT
{
    public class TokenOptions
    {
        public string Audience { get; set; } = "Coino";
        public string Issuer { get; set; } = "Shoping.com";
        public int AccessTokenExpiration { get; set; } = 10;
        public int RefreshTokenExpiration { get; set; } = 1440;

        public string SecurityKey { get; set; } = "mysupersecretkeymysupersecretkey";
    }
}
