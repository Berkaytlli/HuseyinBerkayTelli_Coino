using Entity.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }
        public void Seed()
        {
            var operationClaims = new OperationClaim[]
            {
            new OperationClaim(){ Id = 1, Name = "Member" },
            new OperationClaim(){Id = 2 ,Name = "Admin"}
            };
            modelBuilder.Entity<OperationClaim>().HasData(operationClaims);
        }
    }
}
