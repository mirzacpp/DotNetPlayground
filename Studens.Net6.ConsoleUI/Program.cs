using Studens.Commons.Utils;

var x = 4;
var y = 2;
var mural = "CJCJ";
var total = 0;
char[] solution = new char[mural.Length];


// Replace all ?
for (int i = 0; i < mural.Length; i++)
{
    if (mural[i] == '?')
    {
        if (i + 1 < mural.Length)
        {
            // If next is also ?, find first alpha
            // Do this only for first element
            if (mural[i + 1] == '?')
            {
                if (i == 0)
                {
                    for (int j = i + 1; j < mural.Length; j++)
                    {
                        if (mural[j] != '?')
                        {
                            solution[i] = mural[j];
                            break;
                        }
                    }
                }
                else
                {
                    // Assign prev
                    solution[i] = solution[i - 1];
                }
            }            
            else
            {
                // Is alpha
                solution[i] = mural[i + 1];
            }
        }
        else
        {
            solution[i] = solution[i - 1];
        }
    }
    else
    {
        // Apend alpha
        solution[i] = mural[i];
    }
}

for (int i = 0; i < solution.Length; i++)
{
    if (i + 1 < solution.Length)
    {
        if (solution[i] == 'C' && solution[i + 1] == 'J')
        {
            total += x;
        }
        else if (solution[i] == 'J' && solution[i + 1] == 'C')
        {
            total += y;
        }
    }
}

Console.WriteLine("X: " + x);
Console.WriteLine("Y: " + y);
Console.WriteLine("Mural: " + mural);
Console.WriteLine("Solution: " + String.Join(",", solution));
Console.WriteLine("Cost: " + total);
Console.ReadKey();


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




