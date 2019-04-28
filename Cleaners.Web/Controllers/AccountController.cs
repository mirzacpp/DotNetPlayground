using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Localization;
using Cleaners.Web.Models.Account;
using Cleaners.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IdentityConfig _identityConfig;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IStringLocalizer<AccountController> localizer,
            IUserService userService,
            IdentityConfig identityConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _localizer = localizer;
            _identityConfig = identityConfig;
            _userService = userService;
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

            var result = await _signInManager
                .PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: _identityConfig.DefaultOptions.LockoutEnabled);

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

        /// <summary>
        /// Returns profile page for current user
        /// </summary>
        [HttpGet("profile", Name = AccountRoutes.Profile)]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<User, UserDetailsModel>(user);

            return View(nameof(Profile), model);
        }

        /// <summary>
        /// Returns page for current user password change
        /// </summary>
        [HttpGet("change-password", Name = AccountRoutes.ChangePassword)]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(nameof(ChangePassword));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ChangePassword), model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            // Check if current password is correct
            if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                ModelState.AddModelError(_localizer[ResourceKeys.InvalidCurrentPassword]);

                return View(nameof(ChangePassword), model);
            }

            var result = await _userService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Return user back to profile page
                return RedirectToRoute(AccountRoutes.Profile);
            }

            ModelState.AddModelErrors(result.Errors.Select(e => e.Description));

            return View(nameof(ChangePassword));
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
            ModelState.AddModelErrorIf(!result.IsLockedOut && !result.IsNotAllowed && !result.RequiresTwoFactor, _localizer[ResourceKeys.InvalidLoginData]);
        }

        #endregion Utils
    }
}