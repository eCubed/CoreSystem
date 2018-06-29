using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using FCore.ResourceServer;
using FCore.WebApiServerBase;
using FCore.Cryptography;
using FCore.ResourceServerTest.Providers;
using FCore.ResourceServerTest.Models;

namespace FCore.ResourceServerTest
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
            services.AddMvc();

            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            });

            services.AddCors();

            services.AddSingleton<ICrypter, Crypt>();
            services.AddTransient<IApiClientProvider<ApiClient>, ApiClientProvider>();
            services.AddTransient<IApiClientHasher, ApiClientHasher>();
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

            app.UseApiKeyMiddleware<ApiClient>(new ApiKeyMiddlewareOptions());

            ResourceServerOptions rsOptions = new ResourceServerOptions();
            rsOptions.CryptionKey = "my-babys-got-a-secret";
            rsOptions.Issuer = "New FCore Issuer";

            //app.UseResourceServerMiddleware(rsOptions);
            app.UseJwtResourceServerMiddleware(rsOptions);
            app.UseMvc();

            app.UseErrorWrappingMiddleware();
        }
    }
}
