using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OtpKeyRepository
{
    public class OtpKeyRepositoryService : BaseRepository<GeneratedOtpKey>, IOtpKeyRepositoryService
    {
        public OtpKeyRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
    }
}
