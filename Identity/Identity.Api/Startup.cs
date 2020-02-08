using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.Api
{
    public class Startup
    {
        private IConfiguration configuration { get; }
        private IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string conn = configuration.GetConnectionString("Identity");
            services.AddDbContext<AppIdentityDbContext>(options => 
            {
                options.UseMySql(configuration.GetConnectionString("Identity"));
            });

            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

            var builder = services.AddIdentityServer()
              // this adds the operational data from DB (codes, tokens, consents)
              .AddOperationalStore(options =>
              {
                  options.ConfigureDbContext = options => options.UseMySql(configuration.GetConnectionString("Identity"));
                  // this enables automatic token cleanup. this is optional.
                  options.EnableTokenCleanup = true;
                  options.TokenCleanupInterval = 30; // interval in seconds
              })
              .AddInMemoryIdentityResources(ResourceConfig.GetIdentityResources())
              .AddInMemoryApiResources(ResourceConfig.GetApiResources())
              .AddInMemoryClients(ResourceConfig.GetClients(""))
              .AddAspNetIdentity<AppUser>();

            if (environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
