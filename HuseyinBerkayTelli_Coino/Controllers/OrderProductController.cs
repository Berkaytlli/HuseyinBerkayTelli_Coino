using AppEnvironment;
using Authentication.Extensions;
using Business.OrderProductBusiness;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IOrderProductBusinessService _orderProductBusinessService;
        public OrderProductController(IOrderProductBusinessService orderProductBusinessService)
        {
            _orderProductBusinessService= orderProductBusinessService;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] OrderProductVM model)
        {
            var create = _orderProductBusinessService.Create(model, User.GetUserId());
            if (!create.IsSuccess)
            {
                return BadRequest(create.Message ?? MessageType.InsertFailed.ToString());
            }
            return Ok(create.Data);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] OrderProductVM model)
        {
            var edit = _orderProductBusinessService.Edit(id, model, User.GetUserId());
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
            var remove = _orderProductBusinessService.Remove(id, User.GetUserId());
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
            var getAll = _orderProductBusinessService.GetAll();
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
            var getById = _orderProductBusinessService.GetById(id);
            if (!getById.IsSuccess)
            {
                return BadRequest(getById.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(getById.Data);
        }
        [HttpPost]
        [Route("Order/{id:int}")]
        public IActionResult Order(int id)
        {
            var order = _orderProductBusinessService.Order(id, User.GetUserId());
            if (!order.IsSuccess)
            {
                return BadRequest(order.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(order);
        }
    }
}
