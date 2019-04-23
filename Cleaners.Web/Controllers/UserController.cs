using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Models.Users;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for user
    /// </summary>
    [Route("users")]
    public class UserController : FealControllerBase
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IUserModelService _userModelService;
        private readonly IMapper _mapper;

        #endregion Fields

        public UserController(UserManager<User> userManager, IUserService userService, IUserModelService userModelService, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userModelService = userModelService ?? throw new ArgumentNullException(nameof(userModelService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

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
            await _userModelService.PrepareModel(model);

            return View(model: model);
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