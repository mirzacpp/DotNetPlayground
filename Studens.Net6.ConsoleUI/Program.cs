using Studens.Commons.Utils;
using Studens.Net6.ConsoleUI;

var test = new RecordModel
{
    FirstName = "Mirza",
    LastName = "Cupina"
};

var (f, l) = test;

Console.WriteLine(f + " " + l);


//using var watcher = new FileSystemWatcher(@"C:\ITO");

//watcher.NotifyFilter = NotifyFilters.Attributes
//                                | NotifyFilters.CreationTime
//                                | NotifyFilters.DirectoryName
//                                | NotifyFilters.FileName
//                                | NotifyFilters.LastAccess
//                                | NotifyFilters.LastWrite
//                                | NotifyFilters.Security
//                                | NotifyFilters.Size;

//watcher.Changed += OnChanged;
////watcher.Created += OnCreated;
//watcher.Deleted += OnDeleted;
////watcher.Renamed += OnRenamed;
////watcher.Error += OnError;

//watcher.Filter = "*.txt";
//watcher.IncludeSubdirectories = true;
//watcher.EnableRaisingEvents = true;


//void OnChanged(object sender, FileSystemEventArgs e)
//{
//    Console.WriteLine($"Change detected for {e.FullPath}");
//}

//void OnDeleted(object sender, FileSystemEventArgs e)
//{
//    Console.WriteLine($"Delete detected for {e.FullPath}");
//}

Console.ReadLine();




