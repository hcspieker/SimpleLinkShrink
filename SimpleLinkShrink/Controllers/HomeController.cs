using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrink.Data;
using SimpleLinkShrink.Exceptions;
using SimpleLinkShrink.Extensions;
using SimpleLinkShrink.Models;

namespace SimpleLinkShrink.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUrl(ShortlinkCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), model);

            if (!model.TargetUrl!.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.TargetUrl), "The Link field is not a valid fully-qualified http, or https URL.");
                return View(nameof(Index), model);
            }

            var result = await _repository.Create(model.TargetUrl!);

            return RedirectToAction(nameof(State), new { alias = result.Alias });
        }

        [Route("State/{alias}")]
        public async Task<ActionResult> State(string alias)
        {
            try
            {
                var result = await _repository.Get(alias);

                var model = new ShortlinkDetailViewModel
                {
                    Id = result.Id,
                    TargetUrl = result.TargetUrl,
                    ShortlinkUrl = new Uri(Request.GetBaseUrl(), $"s/{result.Alias}").ToString(),
                    StatusUrl = Request.GetDisplayUrl(),
                    ExpirationDate = result.ExpirationDate
                };

                return View(model);
            }
            catch (ShortlinkNotFoundException)
            {
                return RedirectToAction(nameof(PageNotFound));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);

                return View();
            }
            catch (ShortlinkNotFoundException)
            {
                return RedirectToAction(nameof(PageNotFound));
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
