using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Alerts;
using Cleaners.Web.Models.Users;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IUserModelService _userModelService;
        private readonly IMapper _mapper;
        private readonly IAlertManager _alertManager;
        private readonly IStringLocalizer<UserController> _localizer;

        public UserController(UserManager<User> userManager, IUserService userService, IUserModelService userModelService, IMapper mapper, IAlertManager alertManager, IStringLocalizer<UserController> localizer)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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

            return View(model: model);
        }

        [HttpGet("create", Name = UserRoutes.Create)]
        public async Task<IActionResult> Create()
        {
            var model = new UserCreateModel();

            await _userModelService.PrepareModel(model);

            return View(model: model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            // Local method so we don't repeat ourselves
            async Task<IActionResult> ReturnResult()
            {
                await _userModelService.PrepareModel(model);

                return View(model: model);
            }

            if (!ModelState.IsValid)
            {
                return await ReturnResult();
            }

            var user = _mapper.Map<UserCreateModel, User>(model);
            var result = await _userService.CreateAsync(user, model.Password);

            // Create notification for success
            if (result.Succeeded)
            {
                return RedirectToRoute(UserRoutes.Index);
            }

            ModelState.AddModelErrors(result.Errors.Select(e => e.Description));

            return await ReturnResult();
        }

        // Implementiraj deaktivaciju/aktivaciju
        // -||- ručnu potvrdu računa
        // -||- create, update, delete
        // -||- Promjenu lozinke za trenutnog korisnika
        // -||-

        //[HttpGet("create")]
        //public IActionResult Create()
        //{
        //    var res = userManager.CreateAsync(new User
        //    {
        //        Email = "mirza@ito.ba",
        //        UserName = "mirza@ito.ba",
        //        FirstName = "Miki",
        //        LastName = "Lauda",
        //        IsActive = true,
        //        IsDeleted = false,
        //        EmailConfirmed = true,
        //        CreationDateUtc = DateTime.UtcNow,
        //    }, "Pass123$").Result;

        //    if (res.Succeeded)
        //    {
        //        return Content("Ok");
        //    }

        //    return Content(string.Join(", ", res.Errors.Select(e => e.Description)));
        //}
    }
}