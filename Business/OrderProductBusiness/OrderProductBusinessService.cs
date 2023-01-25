using AppEnvironment;
using Context;
using Entity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Repository.CartRepository;
using Repository.OrderProductRepository;
using Repository.ProductRepository;
using Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.OrderProductBusiness
{
    public class OrderProductBusinessService : IOrderProductBusinessService
    {
        private readonly IOrderProductRepositoryService _orderProductRepositoryService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepositoryService _userRepositoryService;
        private readonly IProductRepositoryService _productRepositoryService;
        private readonly ICartRepositoryService _cartRepositoryService;

        public OrderProductBusinessService(IOrderProductRepositoryService orderProductRepositoryService, ApplicationDbContext dbContext, IUserRepositoryService userRepositoryService, IProductRepositoryService productRepositoryService, ICartRepositoryService cartRepositoryService)
        {
            _orderProductRepositoryService = orderProductRepositoryService;
            _dbContext = dbContext;
            _userRepositoryService = userRepositoryService;
            _productRepositoryService = productRepositoryService;
            _cartRepositoryService = cartRepositoryService;
        }

        public Result<OrderProduct> Create(OrderProductVM model, int? operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var orderProduct = new OrderProduct
                {
                    
                    ProductId = model.ProductId,
                    Stock = model.Stock,
                    OrderPrice = model.Price
                };

                var result = _orderProductRepositoryService.Add(orderProduct, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<OrderProduct>(e);
            }
        }
        public Result<OrderProduct> Edit(int id, OrderProductVM model, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getfirst = _orderProductRepositoryService.GetFirst(u => u.Id == id);
                if (!getfirst.IsSuccess)
                {
                    return new Result<OrderProduct>(getfirst.MessageType ?? MessageType.RecordNotFound);
                }
                getfirst.Data.ProductId = model.ProductId;
                getfirst.Data.Stock = model.Stock;
                getfirst.Data.OrderPrice = model.Price;
                
                var result = _orderProductRepositoryService.Update(getfirst.Data, operatorUserId);
                transaction.Commit();
                return result;

            }
            catch (Exception e)
            {

                return new Result<OrderProduct>(e);
            }
        }
        public Result Remove(int id, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var orderProductExists = _orderProductRepositoryService.GetFirst(u => u.Id == id);
                if (!orderProductExists.IsSuccess)
                {
                    return new Result(orderProductExists.MessageType ?? MessageType.RecordNotFound);
                }

                var result = _orderProductRepositoryService.Delete(orderProductExists.Data, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }
        public Result<List<OrderProduct>> GetAll()
        {
            try
            {
                var allOrderProducts = _orderProductRepositoryService.Get(whereCondition: u => u.DeletedBy == null).ToList();
                if (!allOrderProducts.Any())
                {
                    return new Result<List<OrderProduct>>(MessageType.RecordNotFound);
                }
                return new Result<List<OrderProduct>>(allOrderProducts);
            }
            catch (Exception e)
            {
                return new Result<List<OrderProduct>>(e);
            }
        }

        public Result<OrderProduct> GetById(int id)
        {
            try
            {
                var orderProduct = _orderProductRepositoryService.GetFirst(u => u.Id == id);
                if (!orderProduct.IsSuccess)
                {
                    return new Result<OrderProduct>(orderProduct.MessageType ?? MessageType.RecordNotFound);
                }
                return orderProduct;
            }
            catch (Exception e)
            {
                return new Result<OrderProduct>(e);
            }
        }
        public Result Order(int userId,int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var customer = _userRepositoryService.GetFirst(c => c.Id == userId);
                if (!customer.IsSuccess)
                {
                    return new Result(customer.MessageType ?? MessageType.RecordNotFound);
                }

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
                    return new Result(MessageType.RecordNotFound);
                }

                var orderTotal = cartProducts.Sum(cp => cp.TotalPrice);
                if (customer.Data.Balance < orderTotal)
                {
                    return new Result(MessageType.InsufficientBalance);
                }

                foreach (var cartProduct in cartProducts)
                {
                    var product = _productRepositoryService.GetFirst(p => p.Id == cartProduct.ProductId);
                    if (!product.IsSuccess)
                    {
                        return new Result(product.MessageType ?? MessageType.RecordNotFound);
                    }

                    if (product.Data.Stock < cartProduct.Count)
                    {
                        return new Result(MessageType.StockNotEnough);
                    }
                }

                customer.Data.Balance -= orderTotal;
                _userRepositoryService.Update(customer.Data,operatorUserId);

                foreach (var cartProduct in cartProducts)
                {
                    var product = _productRepositoryService.GetFirst(p => p.Id == cartProduct.ProductId);
                    product.Data.Stock -= cartProduct.Count;
                    _productRepositoryService.Update(product.Data,operatorUserId);
                }

                var order = new OrderProduct
                {
                    UserAppId = userId,
                    TotalAmount = orderTotal,
                    OrderDate = DateTime.Now
                };
                _orderProductRepositoryService.Add(order,operatorUserId);

                var orderProducts = cartProducts.Select(cp => new OrderProduct
                {
                    Id = order.Id,
                    ProductId = cp.ProductId,
                    Stock = cp.Count,
                    OrderPrice = cp.Price
                });
                _orderProductRepositoryService.AddRange(orderProducts);

                var cartItems = _cartRepositoryService.Get(c => c.UserAppId == userId);
                _cartRepositoryService.DeleteRange(cartItems);

                transaction.Commit();
                return new Result(MessageType.OperationSuccess);
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }




    }

}

