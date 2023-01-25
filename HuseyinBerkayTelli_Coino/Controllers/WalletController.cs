using AppEnvironment;
using Authentication.Extensions;
using Business.WalletBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using ViewModel;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletBusinessService _walletBusinessService;
        public WalletController(IWalletBusinessService walletBusinessService)
        {
            _walletBusinessService = walletBusinessService;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] WalletVM model)
        {
            var create = _walletBusinessService.Create(model, User.GetUserId());
            if (!create.IsSuccess)
            {
                return BadRequest(create.Message ?? MessageType.InsertFailed.ToString());
            }
            return Ok(create.Data);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] WalletVM model)
        {
            var edit = _walletBusinessService.Edit(id, model, User.GetUserId());
            if (!edit.IsSuccess)
            {
                return BadRequest(edit.Message ?? MessageType.UpdateFailed.ToString());
            }
            return Ok(edit.Data);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var remove = _walletBusinessService.Remove(id, User.GetUserId());
            if (!remove.IsSuccess)
            {
                return BadRequest(remove.Message ?? MessageType.DeleteFailed.ToString());
            }
            return Ok(remove);
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            var getAll = _walletBusinessService.GetAll();
            if (!getAll.IsSuccess)
            {
                return BadRequest(getAll.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getAll.Data);
        }

        [HttpGet]
        [Route("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var getById = _walletBusinessService.GetById(id);
            if (!getById.IsSuccess)
            {
                return BadRequest(getById.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getById.Data);
        }
        [HttpPut]
        [Route("AddBalance")]
        public IActionResult AddBalance(int UserId, decimal Amount)
        {

            var addballace = _walletBusinessService.AddBalance(UserId,Amount);
            if (!addballace.IsSuccess)
            {
                return BadRequest(addballace.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(addballace.Data);

        }
        [HttpPut]
        [Route("SubtractBalance")]
        public IActionResult SubtractBalance(int UserId, decimal Amount)
        {

            var subtractBalance = _walletBusinessService.SubtractBalance(UserId, Amount, User.GetUserId());
            if (!subtractBalance.IsSuccess)
            {
                return BadRequest(subtractBalance.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(subtractBalance.Data);

        }
        [HttpPut]
        [Route("GetUserBalance/{UserId:int}")]
        public IActionResult GetUserBalance(int UserId)
        {

            var getUserBalance = _walletBusinessService.GetUserBalance(UserId);
            if (!getUserBalance.IsSuccess)
            {
                return BadRequest(getUserBalance.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getUserBalance.Data);

        }
    }
}