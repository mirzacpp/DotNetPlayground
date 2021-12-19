int x = 4;
int y = 2;
string mural = "CJCJ";
var sum = 0;

for (int i = 0; i < mural.Length; i++)
{
    for (int j = i + 1; j < mural.Length; j++)
    {
        if (mural[j] != '?')
        {
            if (mural[i] == 'C' && mural[j] == 'J')
                sum += x;
            else if (mural[i] == 'J' && mural[j] == 'C')
                sum += y;
            i = j;
        }
    }
}

Console.WriteLine(sum);

//Console.WriteLine(String.Join(",", solution));

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