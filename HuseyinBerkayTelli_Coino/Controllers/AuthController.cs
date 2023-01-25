using AppEnvironment;
using Business.AuthBusiness;
using Entity.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBusinessService _authBusinessService;
        public AuthController(IAuthBusinessService authBusinessService)
        {
            _authBusinessService = authBusinessService;
        }
        [HttpPost("login")]
        public ActionResult Login(UserForLoginVM userForLoginDto)
        {

            var userToLogin = _authBusinessService.Login(userForLoginDto.Email, userForLoginDto.Password);
            if (!userToLogin.IsSuccess)
                return BadRequest(userToLogin.Message ?? MessageType.OperationFailed.ToString());
            userToLogin.Message = MessageType.OperationSuccess.ToString();
            return Ok(userToLogin);

        }
        [HttpPost("register")]
        public ActionResult Register(UserForRegisterVM userForRegisterDto)
        {
            var registerResponse = _authBusinessService.Register(
                firstname: userForRegisterDto.FirstName,
                lastname: userForRegisterDto.LastName,
                phone: userForRegisterDto.Phone,
                address: userForRegisterDto.Address,
                email: userForRegisterDto.Email,
                password: userForRegisterDto.Password
                );

            if (!registerResponse.IsSuccess)
                return BadRequest(registerResponse.Message ?? MessageType.OperationFailed.ToString());
            registerResponse.Message = MessageType.OperationSuccess.ToString();
            return Ok(registerResponse);
        }
        [HttpPost]
        [Route("refresh/{id:int}")]
        public ActionResult Refresh(int id)
        {
            var getToken = _authBusinessService.CreateNewAccessToken(id);

            if (!getToken.IsSuccess)
            {
                return new ObjectResult(getToken);
            }
            getToken.Message = MessageType.OperationSuccess.ToString();
            return Ok(getToken);
        }
    }
}
