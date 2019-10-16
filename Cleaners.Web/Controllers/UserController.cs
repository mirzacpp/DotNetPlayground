﻿using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Localization;
using Cleaners.Web.Models.Users;
using Cleaners.Web.Services;
using Corvo.AspNetCore.Mvc.UI.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for user
    /// </summary>
    [Route("users")]
    [Authorize(Roles = RoleNames.Admin)]
    public class UserController : CorvoControllerBase
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IUserModelService _userModelService;
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly TempDataAlertManager _tempDataAlertManager;
        private readonly IMapper _mapper;

        #endregion Fields

        public UserController(
            IUserService userService,
            IUserModelService userModelService,
            IStringLocalizer<UserController> localizer,
            TempDataAlertManager tempDataAlertManager,
            IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userModelService = userModelService ?? throw new ArgumentNullException(nameof(userModelService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _tempDataAlertManager = tempDataAlertManager ?? throw new ArgumentNullException(nameof(tempDataAlertManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns home page for users
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = UserRoutes.Index)]
        public IActionResult Index() => View();

        /// <summary>
        /// Returns table data for home page
        /// </summary>
        /// <returns></returns>
        [HttpGet("data", Name = UserRoutes.Data)]
        public IActionResult Data()
        {
            var users = _userService.Get();
            var model = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);

            return PartialView("_Data", model);
        }

        [HttpGet("create", Name = UserRoutes.Create)]
        public async Task<IActionResult> Create()
        {
            var model = new UserCreateModel();

            await _userModelService.PrepareCreateModel(model);

            return View(model: model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            // Local method so we don't repeat ourselves
            async Task<IActionResult> ReturnResult()
            {
                await _userModelService.PrepareCreateModel(model);

                return View(model: model);
            }

            if (!ModelState.IsValid)
            {
                return await ReturnResult();
            }

            var user = _mapper.Map<UserCreateModel, User>(model);
            var createResult = await _userService.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            {
                // Add user to roles after user was successfully created
                var addToRolesResult = await _userService.AddToRolesAsync(user, model.SelectedRoles);

                if (addToRolesResult.Succeeded)
                {
                    _tempDataAlertManager.Success(_localizer[ResourceKeys.CreateRecordSuccessful]);

                    return RedirectToRoute(UserRoutes.Index);
                }

                ModelState.AddModelErrors(addToRolesResult.Errors.GetDescriptions());
            }

            ModelState.AddModelErrors(createResult.Errors.GetDescriptions());

            return await ReturnResult();
        }

        [HttpGet("{id}/update", Name = UserRoutes.Update)]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<User, UserUpdateModel>(user);

            model.SelectedRoles = await _userService.GetRolesAsync(user);

            await _userModelService.PrepareUpdateModel(model);

            return View(model: model);
        }

        [HttpPost("{id}/update")]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            // Local method so we don't repeat ourselves
            async Task<IActionResult> ReturnResult()
            {
                await _userModelService.PrepareUpdateModel(model);

                return View(model: model);
            }

            if (!ModelState.IsValid)
            {
                return await ReturnResult();
            }

            var user = await _userService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user = _mapper.Map<UserUpdateModel, User>(model, user);

            var updateResult = await _userService.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                // Get current roles for selected user
                var currentRoles = await _userService.GetRolesAsync(user);

                // Remove user from these roles
                var removeFromRolesResult = await _userService.RemoveFromRolesAsync(user, currentRoles);

                // If succeeded, add user to currently selected roles
                if (removeFromRolesResult.Succeeded)
                {
                    var addToRolesResult = await _userService.AddToRolesAsync(user, model.SelectedRoles);

                    if (addToRolesResult.Succeeded)
                    {
                        _tempDataAlertManager.Success(_localizer[ResourceKeys.UpdateRecordSuccessful]);

                        return RedirectToRoute(UserRoutes.Index);
                    }

                    ModelState.AddModelErrors(addToRolesResult.Errors.GetDescriptions());
                }

                ModelState.AddModelErrors(removeFromRolesResult.Errors.GetDescriptions());
            }

            ModelState.AddModelErrors(updateResult.Errors.GetDescriptions());

            return await ReturnResult();
        }

        /// <summary>
        /// Returns modal dialog for email confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/confirm-email", Name = UserRoutes.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(int id)
        {
            if (!await _userService.ExistsAsync(id))
            {
                return NotFound();
            }

            var model = new UserConfirmationModel { Id = id };

            return PartialView("_ConfirmEmail", model);
        }

        /// <summary>
        /// Confirms users email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(UserConfirmationModel model)
        {
            var user = await _userService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.ConfirmEmailAsync(user);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.ConfirmEmailSuccessful]);

                return Json(new { redirectUrl = Url.RouteUrl(UserRoutes.Index) });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_ConfirmEmail", model);
        }

        /// <summary>
        /// Returns modal dialog for delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/delete", Name = UserRoutes.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userService.ExistsAsync(id))
            {
                return NotFound();
            }

            var model = new UserConfirmationModel { Id = id };

            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Confirms delete operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(UserConfirmationModel model)
        {
            var user = await _userService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.DeleteAsync(user);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.DeleteRecordSuccessful]);

                return Json(new { redirectUrl = Url.RouteUrl(UserRoutes.Index) });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Returns modal dialog for restore confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/restore", Name = UserRoutes.Restore)]
        public async Task<IActionResult> Restore(int id)
        {
            if (!await _userService.ExistsAsync(id))
            {
                return NotFound();
            }

            var model = new UserConfirmationModel { Id = id };

            return PartialView("_Restore", model);
        }

        /// <summary>
        /// Confirms delete operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(UserConfirmationModel model)
        {
            var user = await _userService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.RestoreAsync(user);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.RestoreRecordSuccessful]);

                return Json(new { redirectUrl = Url.RouteUrl(UserRoutes.Index) });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_Restore", model);
        }

        /// <summary>
        /// Returns page with selected user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/details", Name = UserRoutes.Details)]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<User, UserDetailsModel>(user);

            return View(model);
        }

        [ServiceFilter(typeof(InternalPasswordResetFilter))]
        [HttpGet("{id}/reset-password", Name = UserRoutes.ResetPassword)]
        public async Task<IActionResult> ResetPassword(int id)
        {
            if (!await _userService.ExistsAsync(id))
            {
                return NotFound();
            }

            var model = new UserResetPasswordModel { Id = id };

            return PartialView("_ResetPassword", model);
        }

        [ServiceFilter(typeof(InternalPasswordResetFilter))]
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ResetPassword", model);
            }

            var user = await _userService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.ResetPasswordAsync(user, model.NewPassword);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.ResetPasswordSuccessful]);

                return Json(new { redirectUrl = Url.RouteUrl(UserRoutes.Index) });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_ResetPassword", model);
        }        

        // Implementiraj deaktivaciju/aktivaciju
        // -||- ručnu potvrdu računa
        // -||- create, update, delete
        // -||- Promjenu lozinke za trenutnog korisnika
        // -||- zakljucavanje i otkljucavanje korisnickog racuna
    }
}