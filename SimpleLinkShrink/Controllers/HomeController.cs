using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrink.Data;
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
        public async Task<ActionResult> CreateUrl(CreateShortlinkViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), model);

            if (!model.TargetUrl!.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.TargetUrl), "The Link field is not a valid fully-qualified http, or https URL.");
                return View(nameof(Index), model);
            }

            var createdUrl = await _repository.GenerateShortlink(model.TargetUrl!);

            var result = new CreateShortlinkResultViewModel
            {
                TargetUrl = createdUrl.TargetUrl,
                InternalUrl = new Uri(Request.GetBaseUrl(), $"s/{createdUrl.Alias}").ToString(),
                ExpirationDate = createdUrl.ExpirationDate
            };

            return View("Created", result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
