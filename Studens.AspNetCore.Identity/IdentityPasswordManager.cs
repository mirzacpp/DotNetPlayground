using Studens.AspNetCore.Identity.PasswordGenerator;

namespace Studens.AspNetCore.Identity;

/// <summary>
/// Provides the APIs for managing passwords.
/// TODO: Abstract to interfaces so implementations can be swapped ?
/// Credits to https://www.ryadel.com/en/passwordcheck-c-sharp-password-class-calculate-password-strength-policy-aspnet/
/// </summary>
public class IdentityPasswordManager
{
    public IdentityOptions Options { get; set; }

    /// <summary>
    /// Supported characters for password generator
    /// TODO: Move this to configuration ?
    /// </summary>
    private static readonly string[] _passwordSupportedCharacters = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$"                           // specials
            };

    public IdentityPasswordManager(IOptions<IdentityOptions> optionsAccessor)

    {
        Options = optionsAccessor?.Value ?? new IdentityOptions();
    }

    /// <summary>
    /// Generates random password based on configured <see cref="IdentityOptions"/>
    /// See https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc/
    /// </summary>
    /// <returns>Generated password</returns>
    public virtual string GenerateRandomPassword()
    {
        List<char> chars = new();
        CryptoRandom rand = new();

        if (Options.Password.RequireUppercase)
            chars.Insert(rand.Next(0, chars.Count),
                _passwordSupportedCharacters[0][rand.Next(0, _passwordSupportedCharacters[0].Length)]);

        if (Options.Password.RequireLowercase)
            chars.Insert(rand.Next(0, chars.Count),
                _passwordSupportedCharacters[1][rand.Next(0, _passwordSupportedCharacters[1].Length)]);

        if (Options.Password.RequireDigit)
            chars.Insert(rand.Next(0, chars.Count),
                _passwordSupportedCharacters[2][rand.Next(0, _passwordSupportedCharacters[2].Length)]);

        if (Options.Password.RequireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count),
                _passwordSupportedCharacters[3][rand.Next(0, _passwordSupportedCharacters[3].Length)]);

        // Fill in rest of password
        for (int i = chars.Count; i < Options.Password.RequiredLength
            || chars.Distinct().Count() < Options.Password.RequiredUniqueChars; i++)
        {
            string rcs = _passwordSupportedCharacters[rand.Next(0, _passwordSupportedCharacters.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }

    /// <summary>
    /// Generic method to retrieve password strength: use this for general purpose scenarios,
    /// i.e. when you don't have a strict policy to follow.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public virtual PasswordStrength GetPasswordStrength(string password)
    {
        int score = 0;
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
        if (HasMinimumLength(password, 5)) score++;
        if (HasMinimumLength(password, 8)) score++;
        if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password)) score++;
        if (HasDigit(password)) score++;
        if (HasSpecialChar(password)) score++;
        return (PasswordStrength)score;
    }

    /// <summary>
    /// Sample password policy implementation:
    /// - minimum 8 characters
    /// - at lease one UC letter
    /// - at least one LC letter
    /// - at least one non-letter char (digit OR special char)
    /// </summary>
    /// <returns></returns>
    public virtual bool IsStrongPassword(string password)
    {
        return HasMinimumLength(password, 8)
            && HasUpperCaseLetter(password)
            && HasLowerCaseLetter(password)
            && (HasDigit(password) || HasSpecialChar(password));
    }

    /// <summary>
    /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
    /// </summary>
    public virtual bool IsValidPassword(string password, PasswordOptions opts)
    {
        return IsValidPassword(
            password,
            opts.RequiredLength,
            opts.RequiredUniqueChars,
            opts.RequireNonAlphanumeric,
            opts.RequireLowercase,
            opts.RequireUppercase,
            opts.RequireDigit);
    }

    /// <summary>
    /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
    /// </summary>
    public virtual bool IsValidPassword(
        string password,
        int requiredLength,
        int requiredUniqueChars,
        bool requireNonAlphanumeric,
        bool requireLowercase,
        bool requireUppercase,
        bool requireDigit)
    {
        if (!HasMinimumLength(password, requiredLength)) return false;
        if (!HasMinimumUniqueChars(password, requiredUniqueChars)) return false;
        if (requireNonAlphanumeric && !HasSpecialChar(password)) return false;
        if (requireLowercase && !HasLowerCaseLetter(password)) return false;
        if (requireUppercase && !HasUpperCaseLetter(password)) return false;
        if (requireDigit && !HasDigit(password)) return false;
        return true;
    }

    public virtual bool HasMinimumLength(string password, int minLength)
    {
        return password.Length >= minLength;
    }

    public virtual bool HasMinimumUniqueChars(string password, int minUniqueChars)
    {
        return password.Distinct().Count() >= minUniqueChars;
    }

    /// <summary>
    /// Returns TRUE if the password has at least one digit
    /// </summary>
    public virtual bool HasDigit(string password)
    {
        return password.Any(c => char.IsDigit(c));
    }

    /// <summary>
    /// Returns TRUE if the password has at least one special character
    /// </summary>
    public virtual bool HasSpecialChar(string password)
    {
        // return password.Any(c => char.IsPunctuation(c)) || password.Any(c => char.IsSeparator(c)) || password.Any(c => char.IsSymbol(c));
        return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
    }

    /// <summary>
    /// Returns TRUE if the password has at least one uppercase letter
    /// </summary>
    public virtual bool HasUpperCaseLetter(string password)
    {
        return password.Any(c => char.IsUpper(c));
    }

    /// <summary>
    /// Returns TRUE if the password has at least one lowercase letter
    /// </summary>
    public virtual bool HasLowerCaseLetter(string password)
    {
        return password.Any(c => char.IsLower(c));
    }

    public enum PasswordStrength
    {
        /// <summary>
        /// Blank Password (empty and/or space chars only)
        /// </summary>
        Blank = 0,

        /// <summary>
        /// Either too short (less than 5 chars), one-case letters only or digits only
        /// </summary>
        VeryWeak = 1,

        /// <summary>
        /// At least 5 characters, one strong condition met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Weak = 2,

        /// <summary>
        /// At least 5 characters, two strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Medium = 3,

        /// <summary>
        /// At least 8 characters, three strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Strong = 4,

        /// <summary>
        /// At least 8 characters, all strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        VeryStrong = 5
    }
}