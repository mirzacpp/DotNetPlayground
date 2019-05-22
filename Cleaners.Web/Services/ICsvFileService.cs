using System.Collections;

namespace Cleaners.Web.Services
{
    public interface ICsvFileService
    {
        byte[] GenerateCsv(IEnumerable data);
    }
}