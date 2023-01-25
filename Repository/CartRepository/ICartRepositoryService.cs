using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CartRepository
{
    public interface ICartRepositoryService : IBaseRepositoryService<Cart>
    {
        public void DeleteRange(IEnumerable<Cart> entities);
    }
}
