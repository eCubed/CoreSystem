using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using FCore.AuthServer;
using FCore.AuthServerTest.Providers;
using Microsoft.Extensions.Logging;
using FCore.WebApiServerBase;

namespace FCore.AuthServerTest
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
            services.AddCors();

            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddSingleton<IGrantTypeProcessorFactory, TestGrantTypeProcessorFactory>();
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

            app.UseAuthentication();

            TokenIssuerOptions tokenIssuerOptions = new TokenIssuerOptions();
            tokenIssuerOptions.Issuer = "New FCore Issuer";
            tokenIssuerOptions.CryptionKey = "my-babys-got-a-secret";

            app.UseNewTokenIssuerMiddleware(tokenIssuerOptions);

            app.UseMvc();

            app.UseErrorWrappingMiddleware();
        }
    }
}
