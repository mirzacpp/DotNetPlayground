using Corvo.Consola.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var test = typeof(IRepo2<,>);//.MakeGenericType(typeof(int), typeof(Country));
            var test2 = typeof(IRepo2<int, Country>);

            var array = new[] { typeof(IRepo2<,>), typeof(IRepo<>), typeof(IRepo<City>), typeof(City) };

            foreach (var item in array)
            {
                Console.WriteLine(item.IsGenericTypeDefinition);
            }

            Console.WriteLine("Test:" + test.GetGenericTypeDefinition());
            Console.WriteLine("Test2:" + test2);
        }

        private static void InvokeReflectionSample()
        {
        }

        private static void InvokeDISample()
        {
            var services = new ServiceCollection();

            services.AddSingletonDependencies(typeof(Program).Assembly);

            services.AddSingleton(typeof(IRepo<>), typeof(Repo<>));

            foreach (var item in services)
            {
                Console.WriteLine($"{item.ImplementationType} - {item.ServiceType}");
            }

            var sp = services.BuildServiceProvider(true);

            //var instance = sp.GetRequiredService<IRepo<City>>();
        }

        //private async static Task Main(string[] args)
        //{
        //    #region Snippet

        //    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true);

        //    IConfigurationRoot config = configBuilder.Build();

        //    IServiceCollection services = new ServiceCollection();

        //    services.Configure<ConfigurationTest>(config.GetSection("SectionA"));
        //    services.AddSingleton<IConfigurationTest>(factory => factory.GetRequiredService<IOptions<ConfigurationTest>>().Value);

        //    IConfigurationTest options = services.BuildServiceProvider().GetService<IConfigurationTest>();

        //    //var contextOptions = new DbContextOptionsBuilder<CorvoDbContext>()
        //    //    .EnableDetailedErrors(true)
        //    //    .EnableSensitiveDataLogging(true)
        //    //    .UseSqlServer("Server=.;Database=CorvoDb;Trusted_connection=true");

        //    //using var context = new CorvoDbContext(contextOptions.Options);

        //    //var books = await context.Books.ToListAsync();

        //    //foreach (var item in books)
        //    //{
        //    //    System.Console.WriteLine(item.Title);
        //    //}

        //    //if (!books.Any())
        //    //{
        //    //    System.Console.WriteLine("No books");
        //    //}

        //    // Kreiraj novu knjigu

        //    #endregion Snippet
        //}

        // Minute interval
        //var interval = 60;
        //var now = DateTime.Now.AddMinutes(-90);

        //var start = now.AddMinutes(-now.Minute % interval).AddSeconds(now.Second * -1);
        //var end = now.AddMinutes(now.Minute % interval == 0 ? 0 : interval - now.Minute % interval).AddSeconds(now.Second * -1);

        //Console.WriteLine(start.ToString());
        //    Console.WriteLine(end.ToString());
    }
}