using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Simplicity.Net6.WebApi.Domain
{
    public class ApplicationContext : IdentityAccessTokenDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }      
    }
}