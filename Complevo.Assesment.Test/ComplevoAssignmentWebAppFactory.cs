

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complevo.Assesment.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Complevo.Assesment.Test
{
    internal class ComplevoAssignmentWebAppFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName;
        private readonly Action<ProductContext> _seed;

        public ComplevoAssignmentWebAppFactory(string dbName,Action<ProductContext> seed)
        {
            _dbName = dbName;
            _seed = seed;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductContext>));


                services.Remove(descriptor);

                services.AddDbContext<ProductContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ProductContext>();
                    //var logger = scopedServices
                    //    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    context.Database.EnsureCreated();
                    if (_seed != null)
                    {
                        _seed(context);
                    }
                    context.SaveChangesAsync();


                }
            });
        }
    }
}
