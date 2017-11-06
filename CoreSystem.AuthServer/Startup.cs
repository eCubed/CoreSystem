using CoreLibrary.AuthServer;
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

            services.AddSingleton<ICrypter, Crypt>();
            services.AddTransient<IPasswordCredentialsProvider, PasswordCredentialsProvider>();
            services.AddTransient<IClientCredentialsProvider, ClientCredentialsProvider>();
            services.AddTransient<IPasswordClaimsProvider, PasswordClaimsProvider>();
            services.AddTransient<IClientClaimsProvider, ClientClaimsProvider>();
            services.AddTransient<IAuthServerResponseProvider<CoreSystemAuthServerResponse>, AuthServerResponseProvider>();
            services.AddTransient<IGrantTypeProcessorFactory<CoreSystemAuthServerResponse>, DefaultGrantTypeProcessorFactory<CoreSystemAuthServerResponse>>();

            services.AddCors();

            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });
            
            services.AddCors();

            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });
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
