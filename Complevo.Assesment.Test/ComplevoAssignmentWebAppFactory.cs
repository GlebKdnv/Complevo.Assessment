

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly Action<ApplicationContext> _seed;

        public ComplevoAssignmentWebAppFactory(string dbName,Action<ApplicationContext> seed)
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
                        typeof(DbContextOptions<ApplicationContext>));


                services.Remove(descriptor);

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationContext>();
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

        public HttpClient CreateClientWithToken(string token)
        {
            var client = this.CreateDefaultClient();
            return client.SetAuth(token);
        }
    }
}
