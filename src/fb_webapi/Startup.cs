using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using fb_webapi.Entities;
using Microsoft.EntityFrameworkCore;

using OpenIddict;
using System.Diagnostics;

namespace fb_webapi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(MakeConnectionString()));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict<User, ApplicationDbContext>()
                .EnableTokenEndpoint("/connect/token")
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .DisableHttpsRequirement()
                .AddEphemeralSigningKey();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(options => options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
            );

            app.UseIdentity();

            app.UseOAuthValidation();

            app.UseOpenIddict();

            app.UseMvc();
        }

        private string MakeConnectionString()
        {
            string a = Configuration["dbUsername"];

            return String.Format(
                "Host={0};Database={1};Username={2};Password={3};SSL Mode=Require;Trust Server Certificate=true;",
                Configuration["dbHost"],
                Configuration["dbName"],
                Configuration["dbUser"],
                Configuration["dbPassword"]);
        }
    }
}
