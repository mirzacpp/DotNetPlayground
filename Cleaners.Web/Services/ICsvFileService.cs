using System.Collections;

namespace Cleaners.Web.Services
{
    /// <summary>
    /// Move this to Locker main project and then use Locker.dll for this project
    /// </summary>
    /// <remarks>
    /// TODO: Create attribute so we can flag properties for export ???
    /// </remarks>
    public interface ICsvFileService
    {
        byte[] GenerateCsv(IEnumerable data);
    }
}