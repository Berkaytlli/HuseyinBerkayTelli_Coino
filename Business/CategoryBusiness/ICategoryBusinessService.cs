using AppEnvironment;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.CategoryBusiness
{
    public interface ICategoryBusinessService
    {
        Result<Category> Create(CategoryVM model, int? operatorUserId);
        Result<Category> Edit(int id, CategoryVM ModelDTO, int operatorUserId);
        Result Remove(int id, int operatorUserId);
        Result<List<Category>> GetAll();
        Result<Category> GetById(int id);
    }
}
