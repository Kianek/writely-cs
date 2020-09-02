using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using writely.Data;
using writely.Services;

namespace writely.integration_tests
{
    public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);
                
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddFilter(level => level >= LogLevel.Trace);
                });

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.AddAuthorization();

                services.AddTransient<IAuthService, AuthService>();
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IUserDataService, UserDataService>();
                services.AddTransient<IJournalService, JournalService>();
                services.AddTransient<IEntryService, EntryService>();
                
                var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=false");
                connection.Open();
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connection);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                
                services.Configure<IdentityOptions>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                });

                var sp = services.BuildServiceProvider();
                
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<WebAppFactory<TStartup>>>();

                db.Database.OpenConnection();
                db.Database.EnsureCreated();

                try
                {
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error seeding database: {message}", ex.Message);
                }
            });
        }
    }
}