using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using writely.Data;
using writely.Models;
using writely.Services;

namespace writely
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        
            services.AddControllers();
            
            services.AddTransient<IAuthService, AuthService>();
            
            services.AddTransient<IUserService, UserService>();
            
            services.AddTransient<IUserDataService, UserDataService>();
            
            services.AddTransient<IJournalService, JournalService>();
            
            services.AddTransient<IEntryService, EntryService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseCors(x =>
            {
                x
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials();
            });
            
            app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
