using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studens.Commons.Extensions;

namespace Studens.AspNetCore.Identity.EntityFrameworkCore
{
    /// <inheritdoc/>
    public class IdentityUserStore<TUser> :
        UserStore<TUser>,
        IIdentityUserStore<TUser> where TUser : IdentityUser<string>, new()
    {
        public IdentityUserStore(DbContext context,
            IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public virtual async Task<IEnumerable<TUser>> GetAllAsync(string? userName = null,
        string? email = null,
        CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var query = Users
                .WhereIf(userName.IsNotNullOrEmpty(), p => p.UserName.Contains(userName))
                .WhereIf(email.IsNotNullOrEmpty(), p => p.UserName.Contains(userName));

            return await query.ToListAsync(cancellationToken);
        }
    }
}