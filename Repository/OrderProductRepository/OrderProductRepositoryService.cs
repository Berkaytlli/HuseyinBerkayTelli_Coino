using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OrderProductRepository
{
    public class OrderProductRepositoryService : BaseRepository<OrderProduct>, IOrderProductRepositoryService
    {
        public OrderProductRepositoryService(ApplicationDbContext context) : base(context) 
        {

        }

    }
}
