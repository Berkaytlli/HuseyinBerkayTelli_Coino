using AppEnvironment;
using Authentication.Extensions;
using Business.CartBusiness;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBusinessService _cartBusinessService;
        public CartController(ICartBusinessService cartBusinessService)
        {
            _cartBusinessService= cartBusinessService;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] CartVM model)
        {
            var create = _cartBusinessService.Add(model, User.GetUserId());
            if (!create.IsSuccess)
            {
                return BadRequest(create.Message ?? MessageType.InsertFailed.ToString());
            }
            return Ok(create.Data);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public IActionResult Edit(int id, [FromBody] CartVM model)
        {
            var edit = _cartBusinessService.Update(id, model, User.GetUserId());
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
            var remove = _cartBusinessService.Remove(id, User.GetUserId());
            if (!remove.IsSuccess)
            {
                return BadRequest(remove.Message ?? MessageType.DeleteFailed.ToString());
            }
            return Ok(remove);
        }
        [Route("GetList")]
        [HttpGet]
        public IActionResult GetList()
        {
            var getAll = _cartBusinessService.GetAll();
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
            var getById = _cartBusinessService.GetById(id);
            if (!getById.IsSuccess)
            {
                return BadRequest(getById.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getById.Data);
        }
        [HttpGet]
        [Route("GetCartProductsId/{id:int}")]
        public IActionResult GetCartProductsId(int id)
        {
            var getCartProductsId = _cartBusinessService.GetById(id);
            if (!getCartProductsId.IsSuccess)
            {
                return BadRequest(getCartProductsId.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getCartProductsId.Data);
        }
    }
}
