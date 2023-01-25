using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.WalletRepository
{
    public class WalletRepositoryService : BaseRepository<Wallet>, IWalletRepositoryService
    {
        public WalletRepositoryService(ApplicationDbContext dbContext): base(dbContext)
        {

        }
    }
}
