using Context;
using Entity;
using Microsoft.EntityFrameworkCore;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CartRepository
{
    public class CartRepositoryService : BaseRepository<Cart>, ICartRepositoryService
    {
        private readonly ApplicationDbContext _context;
        public CartRepositoryService(ApplicationDbContext context): base(context)
        {

        }
        public void DeleteRange(IEnumerable<Cart> entities)
        {
            _context.Carts.RemoveRange(entities);
            _context.SaveChanges();
        }

    }
}
