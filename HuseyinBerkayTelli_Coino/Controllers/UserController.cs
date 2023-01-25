using AppEnvironment;
using Business.UserBusiness;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserBusinessService userBusinessService, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _userBusinessService = userBusinessService;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordVM model)
        {
            var result = _userBusinessService.ChangePassword(model, HttpContext.User);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

    }
}
