using AppEnvironment;
using Authentication.Extensions;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimController : ControllerBase
    {
        private readonly IUserOperationClaimBusinessService _userOperationClaim;
        public UserOperationClaimController(IUserOperationClaimBusinessService userOperationClaim)
        {
            _userOperationClaim = userOperationClaim;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(UserOperationClaimVM vM)
        {
            var addClaim = _userOperationClaim.AddClaimToUser(vM, User.GetUserId());
            if (!addClaim.IsSuccess)
                return BadRequest(addClaim.Message ?? MessageType.InsertFailed.ToString());

            return Ok(addClaim.Message ?? MessageType.InsertSuccess.ToString());
        }
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(UserOperationClaimVM vM)
        {
            var deleteClaim = _userOperationClaim.RemoveClaimOfUser(vM, User.GetUserId());
            if (!deleteClaim.IsSuccess)
                return BadRequest(deleteClaim.Message ?? MessageType.DeleteFailed.ToString());

            return Ok(deleteClaim.Message ?? MessageType.DeleteSuccess.ToString());
        }
        [HttpGet]
        [Route("ClaimsOfChoosenUser/{userEmail}")]
        public IActionResult ClaimsOfChoosenUser(string userEmail)
        {
            var list = _userOperationClaim.GetClaimsOfChoosenUser(userEmail);
            if (!list.IsSuccess)
                return BadRequest(list.Message ?? MessageType.RecordNotFound.ToString());

            return Ok(list.Data);
        }
    }
}
