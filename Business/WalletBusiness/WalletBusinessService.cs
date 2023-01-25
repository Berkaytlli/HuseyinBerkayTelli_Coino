using AppEnvironment;
using Context;
using Entity;
using Repository.WalletRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.WalletBusiness
{
    public class WalletBusinessService : IWalletBusinessService
    {
        private readonly IWalletRepositoryService _walletRepositoryService;
        private readonly ApplicationDbContext _dbContext;
        public WalletBusinessService(IWalletRepositoryService walletRepositoryService, ApplicationDbContext dbContext)
        {
            _walletRepositoryService = walletRepositoryService;
            _dbContext = dbContext;
        }
        public Result<Wallet> Create(WalletVM model, int? operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var wallet = new Wallet
                {
                    UserId = model.UserId,
                    Amount = model.Amount,
                };
                var walletExists = _walletRepositoryService.GetFirst(u => u.UserId == model.UserId);
                if (walletExists.IsSuccess)
                {
                    return new Result<Wallet>(walletExists.MessageType ?? MessageType.RecordAlreadyExists);
                }
                var result = _walletRepositoryService.Add(wallet, operatorUserId);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<Wallet>(e);
            }
        }
        public Result<Wallet> Edit(int id, WalletVM model, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getfirst = _walletRepositoryService.GetFirst(u => u.Id == id);
                if (!getfirst.IsSuccess)
                {
                    return new Result<Wallet>(getfirst.MessageType ?? MessageType.RecordNotFound);
                }
                getfirst.Data.Amount = model.Amount;
                var result = _walletRepositoryService.Update(getfirst.Data, operatorUserId);
                transaction.Commit();
                return result;

            }
            catch (Exception e)
            {
                return new Result<Wallet>(e);
            }
        }
        public Result Remove(int id, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var walletExists = _walletRepositoryService.GetFirst(w => w.Id == id);
                if (!walletExists.IsSuccess)
                {
                    return new Result(walletExists.MessageType ?? MessageType.RecordNotFound);
                }

                var resRemove = _walletRepositoryService.Delete(walletExists.Data, operatorUserId);
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
        public Result<List<Wallet>> GetAll()
        {
            try
            {
                var getAllWallet = _walletRepositoryService.Get(whereCondition: u => u.DeletedBy == null).ToList();
                if (!getAllWallet.Any())
                {
                    return new Result<List<Wallet>>(MessageType.RecordNotFound);
                }
                return new Result<List<Wallet>>(getAllWallet);
            }
            catch (Exception e)
            {
                return new Result<List<Wallet>>(e);
            }
        }
        public Result<Wallet> GetById(int id)
        {
            try
            {
                var getWallet = _walletRepositoryService.GetFirst(u => u.Id == id);
                if (!getWallet.IsSuccess)
                {
                    return new Result<Wallet>(MessageType.RecordNotFound);
                }
                return getWallet;
            }
            catch (Exception e)
            {
                return new Result<Wallet>(e);
            }
        }
        public Result<decimal> GetUserBalance(int userId)
        {
            try
            {
                var wallet = _walletRepositoryService.GetFirst(u => u.UserId == userId);
                if (!wallet.IsSuccess)
                {
                    return new Result<decimal>(wallet.MessageType ?? MessageType.RecordNotFound);
                }
                return new Result<decimal>(wallet.Data.Amount);
            }
            catch (Exception e)
            {
                return new Result<decimal>(e);
            }
        }
        public Result<Wallet> AddBalance(int userId, decimal amount)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var wallet = _walletRepositoryService.GetFirst(u => u.UserId == userId);
                if (!wallet.IsSuccess)
                {
                    return new Result<Wallet>(wallet.MessageType ?? MessageType.RecordNotFound);
                }
                wallet.Data.Amount += amount;
                var result = _walletRepositoryService.Update(wallet.Data,1);
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                return new Result<Wallet>(e);
            }
        }
        public Result<Wallet> SubtractBalance(int userId, decimal amount, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var wallet = _walletRepositoryService.GetFirst(w => w.UserId == userId);
                if (!wallet.IsSuccess)
                {
                    return new Result<Wallet>(wallet.MessageType ?? MessageType.RecordNotFound);
                }

                if (wallet.Data.Amount < amount)
                {
                    return new Result<Wallet>(MessageType.InsufficientBalance);
                }

                wallet.Data.Amount -= amount;
                var updateResult = _walletRepositoryService.Update(wallet.Data, operatorUserId);
                if (!updateResult.IsSuccess)
                {
                    return new Result<Wallet>(updateResult.MessageType ?? MessageType.UpdateFailed);
                }

                transaction.Commit();
                return updateResult;
            }
            catch (Exception e)
            {
                return new Result<Wallet>(e);
            }
        }


    }
}
