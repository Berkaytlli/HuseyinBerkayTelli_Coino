using AppEnvironment;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.CartBusiness
{
    public interface ICartBusinessService
    {
        Result<Cart> Add(CartVM model, int operatorUserId);
        Result<Cart> Update(int id, CartVM modelDTO, int operatorUserId);
        Result Remove(int id, int operatorUserId);
        Result<List<Cart>> GetAll();
        Result<Cart> GetById(int id);
    }
}
