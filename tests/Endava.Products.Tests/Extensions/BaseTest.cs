using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Endava.Products.Tests.Extensions
{
    public class BaseTest : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> Factory;
        protected readonly HttpClient Client;

        public BaseTest(WebApplicationFactory<Program> factory, string dbName)
        {
            Factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<ProductsDbContext>>();

                    services.AddDbContext<ProductsDbContext>(options =>
                        options.UseInMemoryDatabase(dbName)
                    );
                });
            });

            Client = Factory.CreateClient();
        }

        protected T Seed<T>(T value)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
            db.Add(value!);
            db.SaveChanges();
            return value;
        }

        protected void Database(Action<ProductsDbContext> action)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

            action(db);
        }
    }
}
