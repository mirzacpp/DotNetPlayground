using Microsoft.AspNetCore.Mvc;
using Simplicity.Api.Services;
using System.Threading.Tasks;

namespace Simplicity.Api.Controllers
{
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAuthors()
        {
            return Ok(await _authorService.GetAsync());
        }
    }
}