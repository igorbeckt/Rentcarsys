using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RentCarSys.Infra.Data.Context;
using RentCarSys.Presentation.API.AutoMapper;

namespace RentCarSys.Test.IntegrationTest.MockData
{
    public class RentCarSysApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<Contexto>));

                services.AddDbContext<Contexto>(options =>
                    options.UseInMemoryDatabase("RentCarSysDatabase", root).ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

                services.AddAutoMapper(typeof(EntitiesDTOMappingProfile));
            });

            return base.CreateHost(builder);
        }
    }
}
