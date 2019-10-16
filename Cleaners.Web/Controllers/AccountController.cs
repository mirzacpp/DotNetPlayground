using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Localization;
using Cleaners.Web.Models.Account;
using Cleaners.Web.Models.Users;
using Corvo.AspNetCore.Mvc.UI.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all account based requests
    /// </summary>
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IdentityConfig _identityConfig;
        private readonly TempDataAlertManager _tempDataAlertManager;

        #endregion Fields

        #region Methods

        public AccountController(UserManager<User> userManager, IUserService userService, SignInManager<User> signInManager, IMapper mapper, IStringLocalizer<AccountController> localizer, IdentityConfig identityConfig, TempDataAlertManager tempDataAlertManager)
        {
            _userManager = userManager;
            _userService = userService;
            _signInManager = signInManager;
            _mapper = mapper;
            _localizer = localizer;
            _identityConfig = identityConfig;
            _tempDataAlertManager = tempDataAlertManager;
        }

        #endregion Methods

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

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            if (!ValidateLoginInput(user))
            {
                return View(model);
            }

            var result = await _signInManager
                .PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: _identityConfig.DefaultOptions.LockoutEnabled);

            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }

            ProcessSignInResult(result);

            return View(model);
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

            return View(model);
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

            return View(new AccountChangePasswordModel());
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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

                return View(model);
            }

            var result = await _userService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.ChangePasswordSuccessful]);

                return RedirectToRoute(AccountRoutes.Profile);
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return View(model);
        }

        [HttpGet("{id}/lock-account", Name = AccountRoutes.LockAccount)]
        public async Task<IActionResult> LockAccount(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new AccountLockoutModel { UserId = id, };

            return PartialView("_LockAccount", model);
        }

        [HttpPost("{id}/lock-account")]
        public async Task<IActionResult> LockAccount(AccountLockoutModel model)
        {
            var user = await _userService.GetByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            // Use shared lockout result variable
            IdentityResult lockoutResult;

            // Local method for both operations bellow
            IActionResult ResultSuccess(string resourceKey)
            {
                _tempDataAlertManager.Success(_localizer[resourceKey]);

                return Json(new { redirectUrl = Url.RouteUrl(UserRoutes.Index) });
            }

            // Disabled lockout if requested
            if (model.LockoutDisabled)
            {
                // We will just pass null to overwrite current lockout time
                lockoutResult = await _userService.LockAccount(user, null);

                if (lockoutResult.Succeeded)
                {
                    return ResultSuccess(ResourceKeys.AccountUnlockSuccessful);
                }
            }
            else
            {
                // Use default lockout time if not specified
                model.LockoutEndUtc = model.LockoutEndUtc != null ?
                    model.LockoutEndUtc :
                    DateTime.UtcNow.Add(_identityConfig.LockoutOptions.DefaultLockoutTimeSpan);

                lockoutResult = await _userService.LockAccount(user, model.LockoutEndUtc.Value);

                if (lockoutResult.Succeeded)
                {
                    return ResultSuccess(ResourceKeys.AccountLockoutSuccessful);
                }
            }

            // Error occured while setting lockout end
            ModelState.AddModelErrors(lockoutResult.Errors.GetDescriptions());

            return PartialView("_LockAccount", model);
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