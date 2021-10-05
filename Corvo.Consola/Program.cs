using Corvo.Consola.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            InvokeDISample();

            Console.ReadKey();
        }

        private static void InvokeReflectionSample()
        {
        }

        private static void InvokeDISample()
        {
            //var types = new[] { typeof(IRepo<City>), typeof(IRepo<>), typeof(IRepo2<,>) };

            //foreach (var type in types)
            //{
            //    Console.WriteLine(type);
            //    Console.WriteLine(type.GetGenericTypeDefinition());
            //    Console.WriteLine("IsGenericType: " + type.IsGenericType);
            //    Console.WriteLine("IsGenericTypeDefinition: " + type.IsGenericTypeDefinition);
            //    Console.WriteLine("ContainsGenericParameters: " + type.ContainsGenericParameters);
            //    Console.WriteLine("IsGenericTypeParameter: " + type.IsGenericTypeParameter);
            //    Console.WriteLine("IsConstructedGenericType: " + type.IsConstructedGenericType);
            //    Console.WriteLine(Environment.NewLine);
            //}

            var services = new ServiceCollection();

            services
                .AddSingletonDependencies(assemblyMarkerTypes: typeof(Program))
                .AddScopedDependencies(assembliesToScan: typeof(Program).Assembly)
                .AddTransientDependencies(assembliesToScan: typeof(Program).Assembly);            

            foreach (var item in services)
            {
                Console.WriteLine($"{item.ImplementationType} - {item.ServiceType} - {item.Lifetime}");
            }

            var sp = services.BuildServiceProvider(true);

            var instance = sp.GetRequiredService<IRepo<City>>();
            var instance2 = sp.GetRequiredService<Repo3<City>>();

            instance.Introduce();
            Console.WriteLine(instance2.WhoAmI());            
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