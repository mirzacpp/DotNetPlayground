using Cleaners.DependencyInjection.Interfaces;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Corvo.Consola
{
    public interface IConfigurationTest
    {
        string KeyA { get; }
        string KeyB { get; }
    }

    public class ConfigurationTest : IConfigurationTest
    {
        public string KeyA { get; set; }
        public string KeyB { get; set; }
    }

    public interface IFoo
    {
        string Name { get; }
    }

    public class FooA : IFoo, IScopedDependency
    {
        public string Name => nameof(FooA);
    }

    public class FooB : IFoo, IScopedDependency
    {
        public string Name => nameof(FooB);
    }

    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var cultureInfo = new CultureInfo("hr");
            string priceDot = "10.50";
            string priceComma = "10,50";

            decimal.TryParse(priceDot, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultDot);
            decimal.TryParse(priceComma.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var resultComma);
            var ok = decimal.Parse(priceComma.Replace(",", "."), CultureInfo.InvariantCulture);

            Console.WriteLine("Dot: " + resultDot);
            Console.WriteLine("Comma: " + resultComma);
            Console.WriteLine("Comma: " + ok);
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