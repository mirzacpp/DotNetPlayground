using Studens.Commons.Utils;


var months = DateTimeUtils.GetMonthsOfYear();
var months2 = DateTimeUtils.GetMonthsOfYear();
Console.WriteLine(String.Join(",", months));
Console.WriteLine(String.Join(",", months2));


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




