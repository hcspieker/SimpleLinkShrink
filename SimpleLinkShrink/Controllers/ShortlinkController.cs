using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrink.Data;
using SimpleLinkShrink.Exceptions;

namespace SimpleLinkShrink.Controllers
{
    [Route("s")]
    [ApiController]
    public class ShortlinkController : ControllerBase
    {
        private readonly IRepository _repository;

        public ShortlinkController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{alias}")]
        public async Task<ActionResult> Get(string alias)
        {
            try
            {
                var result = await _repository.GetTargetUrl(alias);
                return Redirect(result);
            }
            catch (ShortlinkNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
