using AppEnvironment;
using Context;
using Entity;
using Repository.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.ProductBusiness
{
    public class ProductBusinessService : IProductBusinessService
    {
        private readonly IProductRepositoryService _productRepositoryService;
        private readonly ApplicationDbContext _dbContext;
        public ProductBusinessService(IProductRepositoryService productRepositoryService, ApplicationDbContext dbContext)
        {
            _productRepositoryService = productRepositoryService;
            _dbContext = dbContext;
        }
        public Result<Product> Create(ProductVM model, int? operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    ProductCode= model.ProductCode,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock,
                    Image = model.Image,
                    
                };
                var productExists = _productRepositoryService.GetFirst(u => u.ProductName == model.ProductName);
                if (productExists.IsSuccess)
                {
                    return new Result<Product>(productExists.MessageType ?? MessageType.RecordAlreadyExists);
                }
                var result = _productRepositoryService.Add(product, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {

                return new Result<Product>(e);
            }
        }

        public Result<Product> Edit(int id, ProductVM ModelDTO, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getfirst = _productRepositoryService.GetFirst(u => u.Id == id);
                if (!getfirst.IsSuccess)
                {
                    return new Result<Product>(getfirst.MessageType ?? MessageType.RecordNotFound);
                }
                getfirst.Data.ProductName = ModelDTO.ProductName;
                getfirst.Data.CategoryId = ModelDTO.CategoryId;
                getfirst.Data.Description = ModelDTO.Description;
                getfirst.Data.Price = ModelDTO.Price;
                getfirst.Data.Stock = ModelDTO.Stock;
                getfirst.Data.ProductCode = ModelDTO.ProductCode;
                getfirst.Data.Image = ModelDTO.Image;

                var result = _productRepositoryService.Update(getfirst.Data, operatorUserId);
                transaction.Commit();
                return result;

            }
            catch (Exception e)
            {

                return new Result<Product>(e);
            }
        }
        public Result Remove(int id, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var productExists = _productRepositoryService.GetFirst(u => u.Id == id);
                if (!productExists.IsSuccess)
                {
                    return new Result(productExists.MessageType ?? MessageType.RecordNotFound);
                }

                var resRemove = _productRepositoryService.Delete(productExists.Data, operatorUserId);
                if (!resRemove.IsSuccess)
                {
                    return new Result(resRemove.MessageType ?? MessageType.DeleteFailed);
                }
                transaction.Commit();
                return resRemove;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }
        public Result<List<Product>> GetAll()
        {
            try
            {
                var getAllProduct = _productRepositoryService.Get(whereCondition: u => u.DeletedBy == null).ToList();
                if (!getAllProduct.Any())
                {
                    return new Result<List<Product>>(MessageType.RecordNotFound);
                }
                return new Result<List<Product>>(getAllProduct);
            }
            catch (Exception e)
            {
                return new Result<List<Product>>(e);
            }
        }
        public Result<Product> GetById(int id)
        {
            try
            {
                var getProduct = _productRepositoryService.GetFirst(u => u.Id == id);
                if (!getProduct.IsSuccess)
                {
                    return new Result<Product>(MessageType.RecordNotFound);
                }
                return getProduct;
            }
            catch (Exception e)
            {
                return new Result<Product>(e);
            }
        }
        public Result<List<Product>> SearchByName(string productName)
        {
            try
            {
                var products = _productRepositoryService.Get(p => p.ProductName.Contains(productName) && p.DeletedBy == null).ToList();
                if (!products.Any())
                {
                    return new Result<List<Product>>(MessageType.RecordNotFound);
                }
                return new Result<List<Product>>(products);
            }
            catch (Exception e)
            {
                return new Result<List<Product>>(e);
            }
        }
    }
}


    

