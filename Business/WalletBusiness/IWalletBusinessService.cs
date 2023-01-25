using AppEnvironment;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.WalletBusiness
{
    public interface IWalletBusinessService
    {
        Result<Wallet> Create(WalletVM model, int? operatorUserId);
        Result<Wallet> Edit(int id, WalletVM model, int operatorUserId);
        Result Remove(int id, int operatorUserId);
        Result<List<Wallet>> GetAll();
        Result<Wallet> GetById(int id);
        Result<decimal> GetUserBalance(int userId);
        Result<Wallet> AddBalance(int userId, decimal amount);
        Result<Wallet> SubtractBalance(int userId, decimal amount, int operatorUserId);

    }
}
