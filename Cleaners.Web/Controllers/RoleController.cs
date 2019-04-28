using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Services.Roles;
using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for roles
    /// </summary>
    [Authorize(Roles = RoleNames.Admin)]
    [Route("roles")]
    public class RoleController : FealControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("", Name = RoleRoutes.Index)]
        public IActionResult Index() => View();

        [HttpGet("create", Name = RoleRoutes.Create)]
        public IActionResult Create() => View(new RoleCreateModel());

        [HttpPost("create")]
        public async Task<IActionResult> Create(RoleCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = _mapper.Map<RoleCreateModel, Role>(model);
            var result = await _roleService.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToRoute(RoleRoutes.Index);
            }

            ModelState.AddModelErrors(result.Errors.Select(e => e.Description));

            return View(model);
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

            return View(model);
        }

        [HttpPost("{id}/update")]
        public async Task<IActionResult> Update(RoleUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve role if already being tracked, so we don't end up with tracking exceptions
            // Note that we also need ConcurrencyStamp column for successful update
            var role = await _roleService.GetByIdAsync(model.Id);

            if (role == null)
            {
                return NotFound();
            }

            role = _mapper.Map<RoleUpdateModel, Role>(model, role);
            var result = await _roleService.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToRoute(RoleRoutes.Index);
            }

            ModelState.AddModelErrors(result.Errors.Select(e => e.Description));

            return View(model);
        }
    }
}