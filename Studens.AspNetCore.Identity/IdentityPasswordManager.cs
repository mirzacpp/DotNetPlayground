using Studens.AspNetCore.Identity.PasswordGenerator;

namespace Studens.AspNetCore.Identity;

/// <summary>
/// Provides the APIs for managing passwords.
/// TODO: Abstract to interfaces so implementations can be swapped ?
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
            "!@$"                        // specials
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
    /// Calculates <paramref name="password"/> strength
    /// </summary>
    /// <param name="password">Password to check</param>
    /// <returns>Password strength</returns>
    public virtual PasswordStrength GetPasswordStrength(string password) => PasswordCheck.GetPasswordStrength(password);

    /// <summary>
    /// Checks if <paramref name="password"/> is valid based on <see cref="PasswordOptions"/>
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>Password valid result</returns>
    public virtual bool IsValidPassword(string password) => PasswordCheck.IsValidPassword(password, Options.Password);
}