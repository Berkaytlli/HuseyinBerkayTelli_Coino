using AppEnvironment;
using Authentication.Extensions;
using Business.CategoryBusiness;
using Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using ViewModel;

namespace HuseyinBerkayTelli_Coino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryBusinessService _categoryBusinessService; 
        public CategoryController(ICategoryBusinessService categoryBusinessService)
        {
            _categoryBusinessService = categoryBusinessService;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(CategoryVM model)
        {

            var createCategory = _categoryBusinessService.Create(model, User.GetUserId());
            if (!createCategory.IsSuccess)
            {
                return BadRequest(createCategory.Message ?? MessageType.RecordAlreadyExists.ToString());
            }
            return Ok(createCategory.Data);
        }
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var delCategory = _categoryBusinessService.Remove(id, User.GetUserId());
            if (!delCategory.IsSuccess)
            {
                return BadRequest(delCategory.Message ?? MessageType.DeleteFailed.ToString());
            }
            return Ok(delCategory.Message ?? MessageType.DeleteSuccess.ToString());
        }
        [HttpPut]
        [Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] CategoryVM request)
        {
            var CategoryUpdate = _categoryBusinessService.Edit(id, request, User.GetUserId());
            if (!CategoryUpdate.IsSuccess)
            {
                return BadRequest(CategoryUpdate.Message ?? MessageType.UpdateFailed.ToString());
            }
            return Ok(CategoryUpdate.Data);

        }
        [HttpGet]
        [Route("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryBusinessService.GetById(id);
            if (!category.IsSuccess)
            {
                return BadRequest(category.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(category.Data);
        }
        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {

            var zone = _categoryBusinessService.GetAll();
            if (!zone.IsSuccess)
            {
                return BadRequest(zone.Message ?? MessageType.RecordNotFound.ToString());
            }
            return Ok(zone.Data);

        }
    }
}
