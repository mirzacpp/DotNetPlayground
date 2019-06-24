using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Roles;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Alerts;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Localization;
using Cleaners.Web.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for roles
    /// </summary>
    [Authorize(Roles = RoleNames.Admin)]
    [Route("roles")]
    public class RoleController : AdminControllerBase
    {
        #region Fields

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly TempDataAlertManager _tempDataAlertManager;
        private readonly IStringLocalizer<RoleController> _localizer;
        private readonly ILogger<RoleController> _logger;

        #endregion Fields

        #region Constructor

        public RoleController(IRoleService roleService, IMapper mapper, TempDataAlertManager tempDataAlertManager, IStringLocalizer<RoleController> localizer, ILogger<RoleController> logger)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tempDataAlertManager = tempDataAlertManager ?? throw new ArgumentNullException(nameof(tempDataAlertManager));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructor

        #region Methods

        [HttpGet("", Name = RoleRoutes.Index)]
        public IActionResult Index() => View();

        [HttpGet("data", Name = RoleRoutes.Data)]
        public IActionResult Data()
        {
            var roles = _roleService.GetAll();
            var model = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleModel>>(roles);

            _logger.LogInformation("Logging data controller");

            return PartialView("_Data", model);
        }

        [HttpGet("create", Name = RoleRoutes.Create)]
        public IActionResult Create() => PartialView("_Create", new RoleCreateModel());

        [HttpPost("create")]
        public async Task<IActionResult> Create(RoleCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }

            var role = _mapper.Map<RoleCreateModel, Role>(model);
            var result = await _roleService.CreateAsync(role);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.CreateRecordSuccessful]);

                return Json(new { redirectUrl = GetIndexUrl() });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_Create", model);
        }

        [HttpGet("{id}/update", Name = RoleRoutes.Update)]
        public async Task<IActionResult> Update(int id)
        {
            var role = await _roleService.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Role, RoleUpdateModel>(role);

            return PartialView("_Update", model: model);
        }

        [HttpPost("{id}/update")]
        public async Task<IActionResult> Update(RoleUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Update", model: model);
            }

            // Retrieve role if already being tracked, so we don't end up with tracking exceptions.
            // Note that we also need ConcurrencyStamp column for successful update.
            var role = await _roleService.GetByIdAsync(model.Id);

            if (role == null)
            {
                return NotFound();
            }

            role = _mapper.Map<RoleUpdateModel, Role>(model, role);
            var result = await _roleService.UpdateAsync(role);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.UpdateRecordSuccessful]);

                return Json(new { redirectUrl = GetIndexUrl() });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_Update", model: model);
        }

        /// <summary>
        /// Returns modal dialog for delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/delete", Name = RoleRoutes.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _roleService.ExistsAsync(id))
            {
                return NotFound();
            }

            var model = new RoleConfirmationModel { Id = id };

            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Confirms delete operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(RoleConfirmationModel model)
        {
            var user = await _roleService.GetByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _roleService.DeleteAsync(user);

            if (result.Succeeded)
            {
                _tempDataAlertManager.Success(_localizer[ResourceKeys.DeleteRecordSuccessful]);

                return Json(new { redirectUrl = GetIndexUrl() });
            }

            ModelState.AddModelErrors(result.Errors.GetDescriptions());

            return PartialView("_Delete", model);
        }

        #endregion Methods

        #region Utils

        [NonAction]
        private string GetIndexUrl() => Url.RouteUrl(RoleRoutes.Index);

        #endregion Utils
    }
}