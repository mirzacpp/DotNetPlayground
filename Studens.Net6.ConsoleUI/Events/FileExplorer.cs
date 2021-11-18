using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studens.Net6.ConsoleUI.Events
{
    public class FileExplorer
    {
        public event EventHandler<FileFouldArgs>? FileFound;

        public void Search()
        {
            foreach (var file in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
            {
                Console.WriteLine(file);
                var args = new FileFouldArgs(file);
                FileFound?.Invoke(this, args);

                if (args.CancelRequested)
                {
                    Console.WriteLine("Canceling");
                    break;
                }                
            }
        }
    }
}
