using Microsoft.AspNetCore.Mvc;

namespace SimpleLinkShrink.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
