using System;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region Snippet

            typeof(DateTime).GetConstructor().Invoke(System.Reflection.BindingFlags)

            var type = Type.GetType(@"System.DateTime,
                            System.Private.CoreLib,
                            Version=4.0.0.0,
                            Culture=neutral,
                            PublicKeyToken=7cec85d7bea7798e");
            var date = (DateTime)Activator.CreateInstance(type);
            date = DateTime.UtcNow;

            Console.WriteLine(date.ToShortDateString());

            #endregion Snippet
        }
    }
}