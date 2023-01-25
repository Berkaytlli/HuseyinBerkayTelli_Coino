using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OrderDetailRepository
{
    public class OrderDetailRepositoryService : BaseRepository<OrderDetail>, IOrderDetailRepositoryService
    {

        public OrderDetailRepositoryService(ApplicationDbContext context): base(context)
        {

        }
    }
}
