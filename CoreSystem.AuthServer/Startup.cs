﻿using CoreLibrary.AuthServer;
using CoreLibrary.Cryptography;
using CoreSystem.AuthServer.Providers;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreSystem.AuthServer
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });

            string connectionString = Configuration["ConnectionString"];

            services.AddDbContext<CoreSystemDbContext>(options =>
            {
                options.UseSqlServer(connectionString, opts => {
                    opts.UseRowNumberForPaging();
                });
            });

            services.AddIdentity<CoreSystemUser, CoreSystemRole>()
                .AddEntityFrameworkStores<CoreSystemDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ICredentialsProvider, SimpleCredentialsProvider>();
            services.AddTransient<IAdditionalClaimsProvider, AdditionalClaimsProvider>();
            services.AddSingleton<ICrypter, Crypt>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            TokenIssuerOptions tokenIssuerOptions = new TokenIssuerOptions();
            tokenIssuerOptions.Issuer = "CoreSystem Issuer";
            tokenIssuerOptions.CryptionKey = Configuration["CryptionKey"];

            app.UseTokenIssuerMiddleware<CoreSystemTokenIssuerMiddleware,CoreSystemAuthServerResponse>(tokenIssuerOptions);

            app.UseMvc();
        }
    }
}
