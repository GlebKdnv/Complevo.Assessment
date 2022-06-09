using Complevo.Assesment.Data.Entities;
using Complevo.Assesment.Data.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complevo.Assesment.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>().HasData(
                UserAccountCryptoUtilities.CreateUserAccount("User1", "password1"));
        }
    }
}
