using Context;
using Entity;
using Repository.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CategoryRepository
{
    public class CategoryRepositoryService : BaseRepository<Category>, ICategoryRepositoryService
    {
        
        public CategoryRepositoryService(ApplicationDbContext context) : base(context)
        {

        }
    }
}
