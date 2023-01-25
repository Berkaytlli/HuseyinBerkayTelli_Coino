using AppEnvironment;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.OrderProductBusiness
{
    public interface IOrderProductBusinessService
    {
        Result<OrderProduct> Create(OrderProductVM model, int? operatorUserId);
        Result<OrderProduct> Edit(int id, OrderProductVM model, int operatorUserId);
        Result Remove(int id, int operatorUserId);
        Result<List<OrderProduct>> GetAll();
        Result<OrderProduct> GetById(int id);
        Result Order(int userId, int operatorUserId);
    }
}
