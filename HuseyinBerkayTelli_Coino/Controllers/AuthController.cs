using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserForRegisterVM model)
        {
            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Email = model.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return Ok(user);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserForLoginVM model)
        {
            if (user.Email != model.Email)
            {
                return BadRequest("UserNotFound");
            }
            if (!VerifyPasswordHash(model.Password,user.PasswordHash,user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }
            return Ok("MyCrazyToken");

        }
        
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var comHash = hmac.ComputeHash(System.Text.ASCIIEncoding.UTF8.GetBytes(password));
                return comHash.SequenceEqual(passwordHash);
            }
        }
    }
}
