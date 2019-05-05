using Microsoft.AspNetCore.Identity;

namespace Cleaners.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Holds identity authentication settings
    /// </summary>
    public class IdentityConfig
    {
        /// <summary>
        /// <see cref="Microsoft.AspNetCore.Identity.LockoutOptions"/>
        /// </summary>
        public LockoutOptions LockoutOptions { get; set; }

        /// <summary>
        /// <see cref="Microsoft.AspNetCore.Identity.PasswordOptions"/>
        /// </summary>
        public PasswordOptions PasswordOptions { get; set; }

        /// <summary>
        /// <see cref="Microsoft.AspNetCore.Identity.SignInOptions"/>
        /// </summary>
        public SignInOptions SignInOptions { get; set; }

        /// <summary>
        /// <see cref="Microsoft.AspNetCore.Identity.UserOptions"/>
        /// </summary>
        public UserOptions UserOptions { get; set; }

        public DefaultOptions DefaultOptions { get; set; }
    }

    // Extended Identity options
    public class DefaultOptions
    {
        public bool LockoutEnabled { get; set; }

        public bool InternalPasswordChangeEnabled { get; set; }
    }
}