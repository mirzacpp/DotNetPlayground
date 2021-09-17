using Corvo.Consola.DI;
using Microsoft.Extensions.DependencyInjection;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            InvokeSharpXmlParsingSample();
            //InvokeXmlParsingSample();

            //var test = typeof(IRepo2<,>);//.MakeGenericType(typeof(int), typeof(Country));
            //var test2 = typeof(IRepo2<int, Country>);
            //
            //var array = new[] { typeof(IRepo2<,>), typeof(IRepo<>), typeof(IRepo<City>), typeof(City) };
            //
            //foreach (var item in array)
            //{
            //    Console.WriteLine(item.IsGenericTypeDefinition);
            //}
            //
            //Console.WriteLine("Test:" + test.GetGenericTypeDefinition());
            //Console.WriteLine("Test2:" + test2);
        }

        private static void InvokeReflectionSample()
        {
        }

        private static void InvokeSharpXmlParsingSample()
        {
            using var stream = new FileStream("Xml/Files/route_2.kml", FileMode.Open);
            var file = KmlFile.Load(stream);
            var root = file.Root as Kml;
            var placeMarks = new List<Placemark>();
            var lineString = new List<LineString>();
            ExtractLineStrings(root.Feature, lineString);

            Console.WriteLine(string.Join(", ", placeMarks.Select(s => s.Name).ToList()));
        }

        private static void ExtractPlacemarks(Feature feature, List<Placemark> placemarks)
        {
            // Is the passed in value a Placemark?
            if (feature is Placemark placemark)
            {
                placemarks.Add(placemark);
            }
            else
            {
                // Is it a Container, as the Container might have a child Placemark?
                if (feature is Container container)
                {
                    // Check each Feature to see if it's a Placemark or another Container
                    foreach (Feature f in container.Features)
                    {
                        ExtractPlacemarks(f, placemarks);
                    }
                }
            }
        }

        private static void ExtractLineStrings(Feature feature, List<LineString> lineStrings)
        {
            // Is the passed in value a Placemark?
            if (feature is Placemark placemark)
            {
                if (placemark.Geometry is LineString lineString)
                {
                    Console.WriteLine(string.Join(", ", lineString.Coordinates.Select(s => s.Longitude.ToString()).ToList()));
                }

                //Console.WriteLine(placemark.Geometry);
            }
            else
            {
                // Is it a Container, as the Container might have a child Placemark?
                if (feature is Container container)
                {
                    // Check each Feature to see if it's a Placemark or another Container
                    foreach (Feature f in container.Features)
                    {
                        ExtractLineStrings(f, lineStrings);
                    }
                }
            }
        }

        private static void InvokeXmlParsingSample()
        {
            //using var stream = new FileStream("Xml/Files/books.xml", FileMode.Open, FileAccess.Read);
            using var stream = new FileStream("Xml/Files/route.kml", FileMode.Open, FileAccess.Read);
            var routeContent = XDocument.Load(stream);

            var ns = XNamespace.Get("http://www.opengis.net/kml/2.2");
            var lineString = routeContent.Element(ns + "kml").Element(ns + "Document").Element("Folder");

            //var lineString = routeContent
            //    .Descendants()
            //    .Skip(1)
            //    .Take(1)
            //    .FirstOrDefault();
            //.Where(p => p.XPathSelectElement("genre").Value == "Computer")
            //.ToList();

            //Console.WriteLine(string.Join(", ", lineString.Take(50).ToList()));
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