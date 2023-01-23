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
        [HttpPost]
        public IActionResult Post(ChangePasswordVM model)
        {
            var result = _userBusinessService.ChangePassword(model, HttpContext.User);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
