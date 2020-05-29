using Simplicity.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplicity.Api.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAsync();
    }
}