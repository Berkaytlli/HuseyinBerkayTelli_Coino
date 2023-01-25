using AppEnvironment;
using Authentication.Extensions;
using Business.ProductBusiness;
using Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductBusinessService _productBusinessService;
        public ProductController(IProductBusinessService productBusinessService)
        {
            _productBusinessService = productBusinessService;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ProductVM model)
        {
            var createProduct = _productBusinessService.Create(model, User.GetUserId());
            if (!createProduct.IsSuccess)
            {
                return BadRequest(createProduct.Message ?? MessageType.RecordAlreadyExists.ToString());
            }
            return Ok(createProduct.Data);
        }
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var delProduct = _productBusinessService.Remove(id, User.GetUserId());
            if (!delProduct.IsSuccess)
            {
                return BadRequest(delProduct.Message ?? MessageType.DeleteFailed.ToString());
            }
            return Ok(delProduct.Message ?? MessageType.DeleteSuccess.ToString());
        }
        [HttpPut]
        [Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] ProductVM request)
        {
            var productUpdate = _productBusinessService.Edit(id, request, User.GetUserId());
            if (!productUpdate.IsSuccess)
            {
                return BadRequest(productUpdate.Message ?? MessageType.UpdateFailed.ToString());
            }
            return Ok(productUpdate.Data);
        }
        [HttpGet]
        [Route("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = _productBusinessService.GetById(id);
            if (!product.IsSuccess)
            {
                return BadRequest(product.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(product.Data);
        }
        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            var products = _productBusinessService.GetAll();
            if (!products.IsSuccess)
            {
                return BadRequest(products.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(products.Data);
        }
    }
}