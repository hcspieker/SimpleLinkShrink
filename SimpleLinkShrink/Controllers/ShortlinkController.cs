using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrink.Data;
using SimpleLinkShrink.Exceptions;
using SimpleLinkShrink.Extensions;

namespace SimpleLinkShrink.Controllers
{
    [ApiController]
    public class ShortlinkController : ControllerBase
    {
        private readonly IRepository _repository;

        public ShortlinkController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("s/{alias}")]
        public async Task<ActionResult> Get(string alias)
        {
            try
            {
                var result = await _repository.Get(alias);
                return Redirect(result.TargetUrl);
            }
            catch (ShortlinkNotFoundException)
            {
                return RedirectToRoute(new
                {
                    controller = "Home",
                    action = "PageNotFound"
                });
            }
        }
    }
}
