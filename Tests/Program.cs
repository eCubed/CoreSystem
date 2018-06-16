using CoreLibrary.Net;
using CoreSystem;
using CoreSystem.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Tests.Entities;

namespace Tests
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            string connectionString = Configuration["ConnectionString"];
            services.AddDbContext<CoreSystemDbContext>(options =>
            {
                options.UseSqlServer(connectionString, opts => {
                    opts.UseRowNumberForPaging();
                });
            });

            //services.AddTransient<IQueueingProvider, DummyQueueingProvider>();

            ServiceProvider = services.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            //var result = WebApiClient.GetAsync<object>("http://localhost:49550/api/values").Result;
            //var result = HttpRequestFactory.GetAsync("http://localhost:49550/api/values").Result;
            //var result = HttpRequestFactory.GetAsync<object>("http://localhost:49550/api/values").Result;

            //var result = HttpRequestFactory.GetTokenAsync<object>("http://localhost:49197/token", "admin", "Aaa000$", "password").Result;

            //var result = HttpRequestFactory.UploadAsync<object>("http://localhost:49943/api/upload", "C:/FlixMLFiles/2017/10/201710172208120017_paella001.jpg").Result;

            //EntitiesTests.CreateTest();
            //EntitiesTests.DeleteTest();
            //EntitiesTests.UpdateTest();

            Configure();
            /*
            CoreSystemDbContext db = ServiceProvider.GetService<CoreSystemDbContext>();
            ContactManager<Contact> contactManager = new ContactManager<Contact>(new ContactStore(db));

            ContactsTests.CreateContactTest(contactManager);

            */
            //ContactsTests.GetAndUpdateTest(contactManager);
            //ContactsTests.GetManyTest(contactManager);
        }
    }
}
