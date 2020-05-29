using Microsoft.EntityFrameworkCore;
using Simplicity.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplicity.Api.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly SimpleDbContext _context;

        public AuthorService(SimpleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAsync()
        {
            return await _context.Authors
                .ToListAsync();
        }
    }
}