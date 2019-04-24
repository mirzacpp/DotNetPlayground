using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Alerts;
using Cleaners.Web.Models.Users;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for user
    /// </summary>
    [Route("users")]
    public class UserController : FealControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserModelService _userModelService;
        private readonly IMapper _mapper;
        private readonly IAlertManager _alertManager;
        private readonly IStringLocalizer<UserController> _localizer;

        public UserController(IUserService userService, IUserModelService userModelService, IMapper mapper, IAlertManager alertManager, IStringLocalizer<UserController> localizer)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userModelService = userModelService ?? throw new ArgumentNullException(nameof(userModelService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _alertManager = alertManager ?? throw new ArgumentNullException(nameof(alertManager));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Returns home page for users
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = UserRoutes.Index)]
        public IActionResult Index()
        {
            var users = _userService.Get();
            var model = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);
            //TempData["notification.key"] = new AlertList { new AlertItem(AlertType.Success, "Ok vlado") };

            return View(model: model);
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
                    // Create notification for success

                    return RedirectToRoute(UserRoutes.Index);
                }

                ModelState.AddModelErrors(addToRolesResult.Errors.Select(e => e.Description));
            }

            ModelState.AddModelErrors(createResult.Errors.Select(e => e.Description));

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

        /// <summary>
        /// Returns modal dialog for users email confirmation
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
                // Append notification and redirect
            }

            ModelState.AddModelErrors(result.Errors.Select(e => e.Description));

            return PartialView("_ConfirmEmail", model);
        }

        // Implementiraj deaktivaciju/aktivaciju
        // -||- ručnu potvrdu računa
        // -||- create, update, delete
        // -||- Promjenu lozinke za trenutnog korisnika
        // -||-
    }
}