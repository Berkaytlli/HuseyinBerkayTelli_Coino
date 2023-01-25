using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ProductRepository
{
    public class ProductRepositoryService : BaseRepository<Product>, IProductRepositoryService
    {
        public ProductRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
    }
}
