using AppEnvironment;
using Business.UserBusinessService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Authentication;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessService _userBusinessService;
        public static IWebHostEnvironment _webHostEnvironment;
        public UserController(IUserBusinessService userBusinessService, IWebHostEnvironment webHostEnvironment)
        {
            _userBusinessService = userBusinessService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordVM model)
        {
            var resetPassword = _userBusinessService.ForgotPassword(model);
            if (!resetPassword.IsSuccess)
                return BadRequest(resetPassword.Message ?? MessageType.OperationFailed.ToString());

            return Ok(resetPassword.Message ?? MessageType.OperationSuccess.ToString());
        }
    }
}
