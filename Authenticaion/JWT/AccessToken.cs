using Entity.Authentication;

namespace Authentication.JWTs
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}