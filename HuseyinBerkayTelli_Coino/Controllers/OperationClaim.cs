using AppEnvironment;
using Business;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimController : ControllerBase
    {
        private readonly IOperationClaimBusinessService _operationClaim;
        public OperationClaimController(IOperationClaimBusinessService operationClaim)
        {
            _operationClaim = operationClaim;
        }
        [HttpPost]
        [Route("Create/{operatorUserId:int}")]
        public IActionResult Create(OperationClaimVM vM, int operatorUserId)
        {
            var addClaim = _operationClaim.CreateClaim(vM, operatorUserId);
            if (!addClaim.IsSuccess)
                return BadRequest(addClaim.Message ?? MessageType.RecordAlreadyExists.ToString());

            return Ok(addClaim.Message ?? MessageType.InsertSuccess.ToString());
        }
        [HttpPut]
        [Route("Update/{claimName}/{operatorUserId:int}")]
        public IActionResult Update(string claimName, OperationClaimVM vM, int operatorUserId)
        {
            var editClaim = _operationClaim.EditClaim(claimName, vM, operatorUserId);
            if (!editClaim.IsSuccess)
                return BadRequest(editClaim.Message ?? MessageType.UpdateFailed.ToString());

            return Ok(editClaim.Message ?? MessageType.UpdateSuccess.ToString());
        }
        [HttpDelete]
        [Route("Delete/{claimName}/{operatorUserId:int}")]
        public IActionResult Delete(string claimName, int operatorUserId)
        {
            var deleteClaim = _operationClaim.RemoveClaim(claimName, operatorUserId);
            if (!deleteClaim.IsSuccess)
                return BadRequest(deleteClaim.Message ?? MessageType.DeleteFailed.ToString());

            return Ok(deleteClaim.Message ?? MessageType.DeleteSuccess.ToString());
        }
        [HttpGet]
        [Route("GetByName/{claimName}")]
        public IActionResult GetClaimByName(string claimName)
        {
            var claim = _operationClaim.GetClaimByName(claimName);
            if (!claim.IsSuccess)
                return BadRequest(claim.Message ?? MessageType.RecordNotFound.ToString());

            return Ok(claim.Data);
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var allClaims = _operationClaim.GetClaims();
            if (!allClaims.IsSuccess)
                return BadRequest(allClaims.Message ?? MessageType.RecordNotFound.ToString());

            return Ok(allClaims.Data);
        }
    }
}
