using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string json = @"
                    {
                      'user': {
                            'firstname': 'Mirza',
                            'lastname': 'Cupina',
                        }
                    }";

            JObject obj1 = JObject.Parse(json);
            Console.WriteLine(obj1["user"]["firstname"]);
            dynamic obj2 = obj1;
            Console.WriteLine(obj2.user.firstname);

            dynamic expando = new ExpandoObject();
            expando.SomeData = "Some data";
            Action<string> action = input => Console.WriteLine("This is input: '{0}'", input);
            expando.FakeMethod = action;
            Console.WriteLine(expando.SomeData);
            expando.FakeMethod("Cao");
            IDictionary<string, object> dictionary = expando;
            Console.WriteLine("Keys: {0}", string.Join(", ", dictionary.Keys));
            dictionary["OtherData"] = "other";
            Console.WriteLine(expando.OtherData);
            Console.WriteLine("Keys: {0}", string.Join(", ", dictionary.Keys));
        }
    }
}