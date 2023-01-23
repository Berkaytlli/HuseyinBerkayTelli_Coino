using Context;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OtpKeyRepositoryService : BaseRepository<GeneratedOtpKey>, IOtpKeyRepositoryService
    {
        public OtpKeyRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
    }
}
