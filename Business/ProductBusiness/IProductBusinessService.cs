using AppEnvironment;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.ProductBusiness
{
    public interface IProductBusinessService
    {
        Result<Product> Create(ProductVM model, int? operatorUserId);
        Result<Product> Edit(int id, ProductVM ModelDTO, int operatorUserId);
        Result Remove(int id, int operatorUserId);
        Result<List<Product>> GetAll();
        Result<Product> GetById(int id);
        Result<List<Product>> SearchByName(string productName);
    }
}
