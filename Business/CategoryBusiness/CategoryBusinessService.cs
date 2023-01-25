using AppEnvironment;
using Context;
using Entity;
using Microsoft.EntityFrameworkCore;
using Repository.CategoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.CategoryBusiness
{
    public class CategoryBusinessService : ICategoryBusinessService
    {
        private readonly ICategoryRepositoryService _categoryRepositoryService;
        private readonly ApplicationDbContext _dbContext;
        public CategoryBusinessService(ICategoryRepositoryService categoryRepositoryService,ApplicationDbContext dbContext)
        {
            _categoryRepositoryService= categoryRepositoryService;
            _dbContext = dbContext;
        }

        public Result<Category> Create(CategoryVM model, int? operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var category = new Category
                {
                    CategoryName= model.CategoryName,
                };
                var categoryExists = _categoryRepositoryService.GetFirst(u => u.CategoryName == model.CategoryName);
                if (categoryExists.IsSuccess)
                {
                    return new Result<Category>(categoryExists.MessageType ?? MessageType.RecordAlreadyExists);
                }
                var result = _categoryRepositoryService.Add(category,operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {

                return new Result<Category>(e);
            }
        }

        public Result<Category> Edit(int id, CategoryVM ModelDTO, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getfirst = _categoryRepositoryService.GetFirst(u=>u.Id == id);
                if (!getfirst.IsSuccess)
                {
                    return new Result<Category>(getfirst.MessageType ?? MessageType.RecordNotFound);
                }
                getfirst.Data.CategoryName = ModelDTO.CategoryName;
                var result = _categoryRepositoryService.Update(getfirst.Data, operatorUserId);
                transaction.Commit();
                return result;

            }
            catch (Exception e)
            {

                return new Result<Category>(e);
            }
        }

        public Result<List<Category>> GetAll()
        {
            try
            {
                var getAllCategory = _categoryRepositoryService.Get(whereCondition: u => u.DeletedBy == null).ToList();
                if (getAllCategory.Any())
                {
                    return new Result<List<Category>>(MessageType.RecordNotFound);
                }
                return new Result<List<Category>>(getAllCategory);
            }
            catch (Exception e)
            {

                return new Result<List<Category>>(e);
            }
        }

        public Result<Category> GetById(int id)
        {
            try
            {
                var getByCategory = _categoryRepositoryService.GetFirst(u => u.Id == id);
                if (!getByCategory.IsSuccess)
                {
                    return new Result<Category>(MessageType.RecordNotFound);

                }
                return getByCategory;
            }
            catch (Exception e)
            {

                return new Result<Category>(e);
            }
        }

        public Result Remove(int id, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var CategoryExists = _categoryRepositoryService.GetFirst(u => u.Id == id);
                if (!CategoryExists.IsSuccess)
                {
                    return new Result<Category>(CategoryExists.MessageType ?? MessageType.RecordNotFound);
                }

                var resRemove = _categoryRepositoryService.Delete(CategoryExists.Data, operatorUserId);
                if (resRemove.MessageType != MessageType.DeleteSuccess)
                    return new Result(resRemove.MessageType ??
                                      MessageType.OperationFailed);
                transaction.Commit();
                return new Result(MessageType.DeleteSuccess);
            }
            catch (Exception e)
            {
                return new Result<Category>(e);
            }
        }
        public Result<List<Category>> SearchByName(string categoryName)
        {
            try
            {
                var search = _categoryRepositoryService.Get(p => p.CategoryName.Contains(categoryName) && p.DeletedBy == null).ToList();
                if (!search.Any())
                {
                    return new Result<List<Category>>(MessageType.RecordNotFound);
                }
                return new Result<List<Category>>(search);
            }
            catch (Exception e)
            {
                return new Result<List<Category>>(e);
            }
        }
    }
}
