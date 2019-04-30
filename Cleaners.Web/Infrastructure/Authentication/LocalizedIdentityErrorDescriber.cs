using Cleaners.Web.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Cleaners.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Enables localization of internal identity error messages
    /// </summary>
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        #region Fields

        private readonly IStringLocalizer<LocalizedIdentityErrorDescriber> _localizer;

        #endregion Fields

        #region Constructor

        public LocalizedIdentityErrorDescriber(IStringLocalizer<LocalizedIdentityErrorDescriber> localizer)
        {
            _localizer = localizer;
        }

        #endregion Constructor

        #region Methods

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = _localizer[ResourceKeys.ConcurrencyFailure]
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = _localizer[ResourceKeys.DefaultError]
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = _localizer[ResourceKeys.DuplicateEmail, email]
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = _localizer[ResourceKeys.DuplicateRoleName, role]
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = _localizer[ResourceKeys.DuplicateUserName, userName]
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = _localizer[ResourceKeys.InvalidEmail, email]
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = _localizer[ResourceKeys.InvalidRoleName, role]
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = _localizer[ResourceKeys.InvalidToken]
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = _localizer[ResourceKeys.InvalidUserName, userName]
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = _localizer[ResourceKeys.LoginAlreadyAssociated]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = _localizer[ResourceKeys.PasswordMismatch]
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = _localizer[ResourceKeys.PasswordRequiresDigit]
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = _localizer[ResourceKeys.PasswordRequiresLower]
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = _localizer[ResourceKeys.PasswordRequiresNonAlphanumeric]
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = _localizer[ResourceKeys.PasswordRequiresUniqueChars, uniqueChars]
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = _localizer[ResourceKeys.PasswordRequiresUpper]
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = _localizer[ResourceKeys.PasswordTooShort, length]
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = _localizer[ResourceKeys.RecoveryCodeRedemptionFailed]
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = _localizer[ResourceKeys.UserAlreadyHasPassword]
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = _localizer[ResourceKeys.UserAlreadyInRole, role]
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = _localizer[ResourceKeys.UserLockoutNotEnabled]
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = _localizer[ResourceKeys.UserNotInRole, role]
            };
        }

        #endregion Methods
    }
}