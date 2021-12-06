using Studens.Commons.Utils;


PriorityQueue<string, int> queue = new PriorityQueue<string, int>();
queue.Enqueue("Item A", 0);
queue.Enqueue("Item B", 60);
queue.Enqueue("Item C", 2);
queue.Enqueue("Item D", 1);

while (queue.TryDequeue(out string item, out int priority))
{
    Console.WriteLine($"Popped Item: {item}. Priority was: {priority}");
}

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




