using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Cleaners.Web.Services
{
    public class CsvFileService : ICsvFileService
    {
        /// <summary>
        /// Generates bytes needed for file generation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] GenerateCsv(IEnumerable data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var writer = new StringWriter();

            // Iterate over collection properties
            foreach (var member in data)
            {
                if (true)
                {
                    Console.WriteLine(2);
                }

                // Use reflection to get data for every property
                foreach (var property in member.GetType().GetProperties())
                {
                    writer.Write(property.GetValue(member, null).ToString() ?? string.Empty);
                    writer.Write(",");
                }
                writer.WriteLine();
            }

            var bytes = Encoding.ASCII.GetBytes(writer.ToString());

            return bytes;
        }
    }
}