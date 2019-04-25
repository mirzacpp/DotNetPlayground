using Cleaners.Core.Domain;
using Cleaners.Web.Configuration;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Localization;
using Cleaners.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("accounts")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IStringLocalizer<AccountController> _localizer;

        #endregion Fields

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IStringLocalizer<AccountController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        #region Methods

        [AllowAnonymous]
        [Route("")]
        [Route("login", Name = AccountRoutes.Login)]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(nameof(Login), model);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Login), model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            if (!ValidateLoginInput(user))
            {
                return View(nameof(Login), model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: AuthenticationDefaults.LockoutEnabled);

            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }

            ProcessSignInResult(result);

            return View(nameof(Login), model);
        }

        [HttpPost("logout", Name = AccountRoutes.Logout)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToRoute(AccountRoutes.Login);
        }

        #endregion Methods

        #region Utils

        [NonAction]
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToRoute(HomeRoutes.Index);
        }

        [NonAction]
        private bool ValidateLoginInput(User user)
        {
            // Required because Identity doesn't check IsActive and IsDeleted values
            if (user == null || user.IsDeleted || !user.IsActive)
            {
                ModelState.AddModelError(_localizer[ResourceKeys.InvalidLoginData]);

                return false;
            }

            return true;
        }

        [NonAction]
        private void ProcessSignInResult(Microsoft.AspNetCore.Identity.SignInResult result)
        {
            ModelState.AddModelErrorIf(result.IsLockedOut, _localizer[ResourceKeys.AccountLockedOut]);
            ModelState.AddModelErrorIf(result.IsNotAllowed, _localizer[ResourceKeys.AccountNotAllowed]);
            ModelState.AddModelErrorIf(result.RequiresTwoFactor, _localizer[ResourceKeys.AccountRequiresTwoFactor]);

            // Required so we don't end up with failed login without message
            if (!result.IsLockedOut && !result.IsNotAllowed && !result.RequiresTwoFactor)
            {
                ModelState.AddModelError(_localizer[ResourceKeys.InvalidLoginData]);
            }
        }

        #endregion Utils
    }
}