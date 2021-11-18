namespace Studens.Net6.ConsoleUI.Events
{
    public class FileFouldArgs : EventArgs
    {
        public string FileName { get; }
        public bool CancelRequested { get; set; }

        public FileFouldArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
