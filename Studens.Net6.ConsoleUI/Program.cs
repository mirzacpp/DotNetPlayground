using Studens.Net6.ConsoleUI.Events;

var filesFound = 0;
EventHandler<FileFouldArgs> onFileFound = (sender, e) =>
{
    if (e.FileName.EndsWith(".json"))
    {
        e.CancelRequested = true;
    }    
};

var fileExplorer = new FileExplorer();
fileExplorer.FileFound += onFileFound;
fileExplorer.Search();
fileExplorer.FileFound -= onFileFound;

Console.WriteLine("Files found " + filesFound);

Console.WriteLine();



