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
                var result = await _repository.GetTargetUrl(alias);
                return Redirect(result);
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

        [HttpGet("d/{alias}")]
        public async Task<ActionResult> Delete(string alias)
        {
            try
            {
                await _repository.DeleteShortlink(alias);
                return Ok($"Removed shortlink {new Uri(Request.GetBaseUrl(), $"s/{alias}").ToString()}");
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
