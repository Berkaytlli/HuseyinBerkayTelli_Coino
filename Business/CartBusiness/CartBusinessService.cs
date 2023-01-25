using AppEnvironment;
using Context;
using Entity;
using Repository.CartRepository;
using Repository.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.CartBusiness
{
    public class CartBusinessService : ICartBusinessService
    {
        private readonly ICartRepositoryService _cartRepositoryService;
        private readonly IProductRepositoryService _productRepositoryService;
        private readonly ApplicationDbContext _dbContext;
        public CartBusinessService(ICartRepositoryService cartRepositoryService, ApplicationDbContext dbContext)
        {
            _cartRepositoryService = cartRepositoryService;
            _dbContext = dbContext;
        }
        public Result<Cart> Add(CartVM model, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var product = _productRepositoryService.GetFirst(p => p.Id == model.ProductId);
                if (!product.IsSuccess)
                {
                    return new Result<Cart>(product.MessageType ?? MessageType.RecordNotFound);
                }

                if (product.Data.Stock < model.Count)
                {
                    return new Result<Cart>(MessageType.StockNotEnough);
                }

                var cart = new Cart
                {
                    UserAppId = model.UserAppId,
                    ProductId = model.ProductId,
                    Count = model.Count,
                    Price = model.Price
                };

                var cartExists = _cartRepositoryService.GetFirst(u => u.ProductId == model.ProductId && u.UserAppId == model.UserAppId);
                if (cartExists.IsSuccess)
                {
                    return new Result<Cart>(cartExists.MessageType ?? MessageType.RecordAlreadyExists);
                }

                var result = _cartRepositoryService.Add(cart, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<Cart>(e);

            }

        }
        public Result<Cart> Update(int id, CartVM modelDTO, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var cart = _cartRepositoryService.GetFirst(u => u.Id == id);
                if (!cart.IsSuccess)
                {
                    return new Result<Cart>(cart.MessageType ?? MessageType.RecordNotFound);
                }

                cart.Data.Count = modelDTO.Count;
                var result = _cartRepositoryService.Update(cart.Data, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<Cart>(e);
            }
        }
        public Result Remove(int id, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var cartExists = _cartRepositoryService.GetFirst(u => u.Id == id);
                if (!cartExists.IsSuccess)
                {
                    return new Result(cartExists.MessageType ?? MessageType.RecordNotFound);
                }

                var result = _cartRepositoryService.Delete(cartExists.Data, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        public Result<List<Cart>> GetAll()
        {
            try
            {
                var getAllCarts = _cartRepositoryService.Get(whereCondition: u => u.DeletedBy == null).ToList();
                if (!getAllCarts.Any())
                {
                    return new Result<List<Cart>>(MessageType.RecordNotFound);
                }
                return new Result<List<Cart>>(getAllCarts);
            }
            catch (Exception e)
            {
                return new Result<List<Cart>>(e);
            }
        }

        public Result<Cart> GetById(int id)
        {
            try
            {
                var getCart = _cartRepositoryService.GetFirst(u => u.Id == id);
                if (!getCart.IsSuccess)
                {
                    return new Result<Cart>(MessageType.RecordNotFound);
                }
                return getCart;
            }
            catch (Exception e)
            {
                return new Result<Cart>(e);
            }
        }
        public Result<List<CartProductViewModel>> GetCartProducts(int userId)
        {
            try
            {
                var cartProducts = (from cart in _dbContext.Carts
                                    join product in _dbContext.Products on cart.ProductId equals product.Id
                                    where cart.UserAppId == userId && cart.DeletedBy == null
                                    select new CartProductViewModel
                                    {
                                        ProductId = cart.ProductId,
                                        ProductName = product.ProductName,
                                        Count = cart.Count,
                                        Price = product.Price,
                                        TotalPrice = cart.Count * product.Price
                                    }).ToList();

                if (!cartProducts.Any())
                {
                    return new Result<List<CartProductViewModel>>(MessageType.RecordNotFound);
                }

                return new Result<List<CartProductViewModel>>(cartProducts);
            }
            catch (Exception e)
            {
                return new Result<List<CartProductViewModel>>(e);
            }
        }


        
    }
}
