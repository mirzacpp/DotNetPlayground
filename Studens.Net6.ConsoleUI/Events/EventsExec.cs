namespace Studens.Net6.ConsoleUI.Events
{
    public static class EventsExec
    {
        public static void Execute()
        {
            using var watcher = new FileSystemWatcher(@"C:\ITO");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                            | NotifyFilters.CreationTime
                                            | NotifyFilters.DirectoryName
                                            | NotifyFilters.FileName
                                            | NotifyFilters.LastAccess
                                            | NotifyFilters.LastWrite
                                            | NotifyFilters.Security
                                            | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            //watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            //watcher.Renamed += OnRenamed;
            //watcher.Error += OnError;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            void OnChanged(object sender, FileSystemEventArgs e)
            {
                Console.WriteLine($"Change detected for {e.FullPath}");
            }

            void OnDeleted(object sender, FileSystemEventArgs e)
            {
                Console.WriteLine($"Delete detected for {e.FullPath}");
            }
        }
    }
}