using DataAccessEF;
using DataAccessEF.Services.Authentication;
using DataAccessEF.Services.LogFile;
using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebAPI.Extensions;

namespace WebAPI
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
            services.AddDbContext<PersonDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default"))
            );
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PersonDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<LogFileService>();

            services.ConfigureSwaggerGen();
            var authConfig = Configuration.GetSection("AuthConfig").Get<AuthConfiguration>();

            if (authConfig != null)
            {
                services.ConfigureAuthentication(authConfig);
                services.AddSingleton(authConfig);
            }
            services.AddControllers();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,LogFileService logger)
        {
            app.ConfigureExceptionHandler(logger);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonTask.Portal v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseGeneratedTokenCheck();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
